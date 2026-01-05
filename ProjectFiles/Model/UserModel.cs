using System.ComponentModel.DataAnnotations;
namespace AgroTechProject.Model;

public class UserModel
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public string? RefreshToken { get; set; } 

    public DateTime RefreshTokenExpiryTime { get; set; }

    public ICollection<ResourceModel> Resources { get; set; } = new List<ResourceModel>();

    public ICollection<BookingModel> Bookings { get; set; } = new List<BookingModel>();
}
