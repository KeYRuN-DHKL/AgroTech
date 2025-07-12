using System.ComponentModel.DataAnnotations;

namespace AgroTechProject.Dtos.AuthDto;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 100 characters.")]
    [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d@$!%*#?&]{6,}$", 
        ErrorMessage = "Password must be at least 6 characters and contain at least one letter and one number.")]
    public required string Password { get; set; }
}