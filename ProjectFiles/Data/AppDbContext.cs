using Microsoft.EntityFrameworkCore;
using AgroTechProject.Model;
using AgroTechProject.Helpers;

namespace AgroTechProject.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserModel> Users { get; set; }
    public DbSet<ResourceModel> Resources { get; set; }
    public DbSet<BookingModel> Bookings { get; set; }
    public DbSet<ReviewModel> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed a default owner user
        modelBuilder.Entity<UserModel>().HasData(new UserModel
        {
            Id = 1,
            FullName = "Kiran Dhakal",
            Email = "kiran@gmail.com",
            PasswordHash = "kiran123",
            PhoneNumber = "9813413252",
            Role = "Admin",
        });

        // Seed a default resource owned by the above user
        modelBuilder.Entity<ResourceModel>().HasData(new ResourceModel
        {
            Id = 1,
            Name = "Tractor",
            Description = "Use for ploughing the field",
            OwnerId = 1,
        });
    }
}