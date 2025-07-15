using AgroTechProject.Dtos.AuthDto;
using AgroTechProject.Dtos.UserDto;


namespace AgroTechProject.Services.Authentication
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);
        Task<UserResponseDto?> GetCurrentUserAsync(int userId);
        Task<AuthResponseDto?> RefreshTokenAsync(TokenDto tokenDto);
    }
}