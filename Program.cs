using Microsoft.EntityFrameworkCore;
using AgroTechProject.Data;
using AgroTechProject.Repositories.BookingRepo;
using AgroTechProject.Repositories.ResourceRepo;
using AgroTechProject.Repositories.ReviewRepo;
using AgroTechProject.Repositories.UserRepo;
using AgroTechProject.Services.Booking;
using AgroTechProject.Services.Resource;
using AgroTechProject.Services.Review;
using AgroTechProject.Services.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS Setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Repositories and Services
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IResourceRepository, ResourceRepository>();
builder.Services.AddScoped<IResourceService, ResourceService>();

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable CORS before routing
app.UseCors("AllowAll");

// Optional: UseHttpsRedirection
// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();