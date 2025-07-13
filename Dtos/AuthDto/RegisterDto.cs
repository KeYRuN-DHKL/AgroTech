using System.ComponentModel.DataAnnotations;

namespace AgroTechProject.Dtos.AuthDto;

public class RegisterDto
{
    [Required(ErrorMessage = "Full name is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Full name must be between 6 and 100 characters.")]
    [RegularExpression(@"^[A-Za-z]+(?:\s[A-Za-z]+)+$", ErrorMessage = "Full name must contain at least two words separated by a space.")]
    public string FullName { get; set; } = null!;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{6,}$", 
        ErrorMessage = "Password must be at least 6 characters and contain at least one letter and one number.")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Role is required.")]
    [RegularExpression("(?i)^(farmer|admin|owner)$",
        ErrorMessage = "Role must be either 'Farmer' or 'Admin' or 'Owner'.")]
    public string Role { get; set; } = null!;
    
    [Required(ErrorMessage = "Phone number is required.")]
    [RegularExpression(@"^(98|97)\d{8}$", ErrorMessage = "Phone number must be exactly 10 digits and start with '98' or '97'.")]
    public string PhoneNumber { get; set; } = null!;
}