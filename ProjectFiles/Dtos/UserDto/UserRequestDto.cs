using System.ComponentModel.DataAnnotations;

namespace AgroTechProject.Dtos.UserDto;

public class UserRequestDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Full name must be between 6 and 100 characters.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    // Usually PasswordHash is not provided by the client directly.
    // Instead, the client sends the plain Password and server hashes it.
    // So consider changing this to Password with validations.
    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    public string Password { get; set; } = null!;

    [RegularExpression("(?i)^(farmer|admin|owner)$", ErrorMessage = "Role must be either 'Farmer' or 'Admin' or 'Owner'.")]
    public string Role { get; set; } = null!;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(98|97)\d{8}$", ErrorMessage = "Phone number must be exactly 10 digits and start with '98' or '97'.")]
    public string PhoneNumber { get; set; } = null!;
}
