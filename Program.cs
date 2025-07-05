using Microsoft.EntityFrameworkCore;
using AgroTechProject.Data;

var builder = WebApplication.CreateBuilder(args);

// Adds services needed for controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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