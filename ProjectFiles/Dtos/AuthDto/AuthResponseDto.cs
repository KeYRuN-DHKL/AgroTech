namespace AgroTechProject.Dtos.AuthDto;
public class AuthResponseDto
{
    public required string Token { get; set; } = null!;
    public required string RefreshToken { get; set; } = null!;
    public required string Role { get; set; } = null!;
    public required string FullName { get; set; } = null!;
}