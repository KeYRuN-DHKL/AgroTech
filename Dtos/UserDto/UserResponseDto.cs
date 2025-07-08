namespace AgroTechProject.Dtos.UserDto;

public class UserResponseDto
{
    public int Id { get; set; }
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
    public required string PhoneNumber { get; set; }
}
