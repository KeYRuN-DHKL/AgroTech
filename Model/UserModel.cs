using System.ComponentModel.DataAnnotations;
namespace AgroTechProject.Model;

public class UserModel
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public required string FullName { get; set; }

    [StringLength(50, MinimumLength = 13, ErrorMessage = "E-mail must be between 15 and 50 characters.")]
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public required string Email { get; set; }

    [StringLength(100, MinimumLength = 3, ErrorMessage = "Password must be between 3 and 20 characters.")]
    [Required]
    public required string PasswordHash { get; set; }
    
    [Required]
    public required string Role { get; set; }
    
    [Phone(ErrorMessage = "Invalid phone number format.")]
    [StringLength(10, ErrorMessage = "Phone number must be 10 digits.")]
    public required string PhoneNumber { get; set; }
    
    [StringLength(200)]
    public string? RefreshToken { get; set; }
    
    public DateTime RefreshTokenExpiryTime { get; set; }

    public  ICollection<ResourceModel> Resources { get; set; }
    public  ICollection<BookingModel> Bookings { get; set; }
}
