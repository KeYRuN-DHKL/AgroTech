namespace AgroTechProject.Dtos.UserDto;

public class UserRequestDto
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string Role { get; set; }
}
