using Microsoft.EntityFrameworkCore;
using AgroTechProject.Data;
using AgroTechProject.Repositories.BookingRepo;
using AgroTechProject.Services.Booking;

var builder = WebApplication.CreateBuilder(args);

// Adds services needed for controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));



var app = builder.Build();

// Enables Swagger in development Environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Optional: app.UseHttpsRedirection(); // Enable later if needed

// Map routes to controller actions
app.UseAuthorization();
app.MapControllers();

app.Run();