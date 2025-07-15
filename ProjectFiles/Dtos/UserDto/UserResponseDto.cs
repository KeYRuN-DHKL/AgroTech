namespace AgroTechProject.Dtos.UserDto;

public class UserResponseDto
{
    public int Id { get; set; }
    public required string FullName { get; set; } = null!;
    public required string Email { get; set; } = null!;
    public required string Role { get; set; } = null!;
    public required string PhoneNumber { get; set; } = null!;
}
