using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using AgroTechProject.Data;
using AgroTechProject.Repositories.BookingRepo;
using AgroTechProject.Repositories.ResourceRepo;
using AgroTechProject.Repositories.ReviewRepo;
using AgroTechProject.Repositories.UserRepo;
using AgroTechProject.Services.Booking;
using AgroTechProject.Services.Resource;
using AgroTechProject.Services.Review;
using AgroTechProject.Services.User;
using AgroTechProject.Services.Authentication;
using AgroTechProject.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

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
        Version = "v1"
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


// Build the app
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.Migrate();
}


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
