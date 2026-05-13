using AgroTechProject.Data;
using AgroTechProject.Repositories.BookingRepo;
using AgroTechProject.Repositories.ResourceRepo;
using AgroTechProject.Repositories.ReviewRepo;
using AgroTechProject.Repositories.UserRepo;
using AgroTechProject.Services.Authentication;
using AgroTechProject.Services.Booking;
using AgroTechProject.Services.Resource;
using AgroTechProject.Services.Review;
using AgroTechProject.Services.User;
using AgroTechProject.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services for controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "AgroTech API",
        Version = "v1",
        Description = "API for managing agricultural data"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter: **Bearer &lt;your JWT token&gt;**"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// CORS Configuration — Allow frontend (React, etc.)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});

// Register DbContext with MySql
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(10,4,32))
    ));

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("JwtSettings"));

// Register repositories
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IResourceService, ResourceService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IUserService, UserService>();

// Register Auth Service
builder.Services.AddScoped<IAuthService, AuthService>();

// JWT Authentication Configuration
var jwtKey = builder.Configuration["JwtSettings:Secret"];
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"];
var jwtAudience = builder.Configuration["JwtSettings:Audience"];



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddControllers(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Host.UseSerilog((context, config) =>
{
    var isDevelopment = context.HostingEnvironment.IsDevelopment();

    //Choice between production or dev
    var environment = isDevelopment ? "Serilog:Development" : "Serilog:Production";

    //These Configuration always reads the value form appsetting serilog section
    var logPath = context.Configuration[$"{environment}:LogPath"];
    var retainedDays = context.Configuration[$"{environment}:RetainedDays"];
    var minimumLevel = context.Configuration[$"{environment}:MinimumLevel"];

    if (isDevelopment)
    {
        //Gets the level of severity from the appsetting.json in the above config code
        config.MinimumLevel.Is(Enum.Parse<LogEventLevel>(minimumLevel));
    }
    else
    {
        config.MinimumLevel.Information();
    }

    //These configuration describes the Application level logs which are noisy and generates lengthy logs. 
    // That's why isn't set to information/warning so it set to Error. The Highest level that is barely possible to occur.
    // The Error only happens when application is running incorrectly

    config.MinimumLevel.Override("Microsoft", LogEventLevel.Error)
          .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Error)
          .MinimumLevel.Override("System", LogEventLevel.Error)

.Enrich.WithMachineName() //Name of the Machine
.Enrich.WithThreadId() //Thead Id where the program is running
.Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName) //Gets Production or Dev
.Enrich.WithProperty("Application", "AgroTechProject") //Custom Property to dispaly applicatin info


//It writes into the console that shows the message just after the application runs

.WriteTo.Console(
    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}: {SourceContext}:{Message}{NewLine}{Exception}]"
    )

    .WriteTo.File(
                logPath,
                rollingInterval: RollingInterval.Day,
                retainedFileCountLimit: Convert.ToInt32(retainedDays),
                fileSizeLimitBytes: 10_000_000,
                rollOnFileSizeLimit: true,
                outputTemplate:
                " Date : {Timestamp:yyyy-MM-dd HH:mm:ss}" 
                + "{NewLine} Severity-Level : [{Level:u3}]"
                + "{NewLine} Source/Controller : {SourceContext}" 
                +" {NewLine} Message : {Message}"
                + "{NewLine} Machine : {MachineName} " 
                + "{NewLine} ThreadId : {ThreadId} "
                + "{NewLine} Environment : {Environment}"
                + "{NewLine} Application: {Application} "
                + "{NewLine} {Exception}"
                +"============================"
                +"{NewLine}"
            );
});

// Build the app
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}

//The below code logs the every http request to every controller;s Actions
//app.UseSerilogRequestLogging(options =>
//{
//    options.MessageTemplate =
//        "HTTP {RequestMethod} {RequestPath} → {StatusCode} in {Elapsed:0.0000}ms";
//});

// Swagger (only in development)
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI();
// }

// Middleware pipeline
// app.UseHttpsRedirection(); // Optional in development

app.UseCors("AllowFrontend");     // Use CORS policy

app.UseAuthentication();          // JWT Auth
app.UseAuthorization();

app.MapControllers();
app.MapHealthChecks("/health");

//var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
//builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

app.Run();
