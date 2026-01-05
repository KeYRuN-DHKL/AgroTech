namespace AgroTechProject.Dtos.AuthDto;
public class AuthResponseDto
{
    public required string Token { get; set; } 
    public required string RefreshToken { get; set; } 
    public required string Role { get; set; } 
    public required string FullName { get; set; } 
}