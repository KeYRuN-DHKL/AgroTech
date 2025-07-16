using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AgroTechProject.Dtos.AuthDto;
using AgroTechProject.Dtos.UserDto;
using AgroTechProject.Helpers;
using AgroTechProject.Model;
using AgroTechProject.Repositories.UserRepo;
using AgroTechProject.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace AgroTechProject.Services.Authentication;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepo;
    private readonly JwtSettings _jwtSettings;
    private readonly ILogger<AuthService> _logger;

    public AuthService(IUserRepository userRepo, IOptions<JwtSettings> jwtSettings, ILogger<AuthService> logger)
    {
        _userRepo = userRepo;
        _jwtSettings = jwtSettings.Value;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> RegisterAsync(RegisterDto dto)
    {
        var existingUser = await _userRepo.GetByEmailAsync(dto.Email);
        if (existingUser != null)
        {
            _logger.LogWarning("Attempted to register with an already existing email: {Email}", dto.Email);
            return null;
        }

        var refreshToken = GenerateRefreshToken();

        var user = new UserModel
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = NormalizeRole(dto.Role),
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
        };

        await _userRepo.AddAsync(user);
        await _userRepo.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            RefreshToken = refreshToken,
            Role = user.Role,
            FullName = user.FullName
        };
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var user = await _userRepo.GetByEmailAsync(dto.Email);
        if (user == null || !PasswordHasher.Verify(user.PasswordHash, dto.Password))
        {
            _logger.LogWarning("Invalid login attempt for email: {Email}", dto.Email);
            return null;
        }

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);
        await _userRepo.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            RefreshToken = user.RefreshToken,
            Role = user.Role,
            FullName = user.FullName
        };
    }

    public async Task<AuthResponseDto?> RefreshTokenAsync(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        if (principal == null)
        {
            _logger.LogWarning("Failed to extract principal from expired token.");
            return null;
        }

        var email = principal.Identity?.Name;
        if (string.IsNullOrEmpty(email)) return null;

        var user = await _userRepo.GetByEmailAsync(email);
        if (user == null || user.RefreshToken != tokenDto.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            _logger.LogWarning("Invalid refresh token attempt for email: {Email}", email);
            return null;
        }

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);
        await _userRepo.SaveChangesAsync();

        return new AuthResponseDto
        {
            Token = GenerateToken(user),
            RefreshToken = user.RefreshToken,
            Role = user.Role,
            FullName = user.FullName
        };
    }

    public async Task<UserResponseDto?> GetCurrentUserAsync(int id)
    {
        var user = await _userRepo.GetByIdAsync(id);
        return user == null ? null : new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber
        };
    }

    private string GenerateToken(UserModel user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.Name, user.Email),
            new Claim("id", user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("name", user.FullName)
        };

        var token = new JwtSecurityToken(
            _jwtSettings.Issuer,
            _jwtSettings.Audience,
            claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            ValidateLifetime = false,
            ValidIssuer = _jwtSettings.Issuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Secret))
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private string NormalizeRole(string? role)
    {
        return role?.Trim().ToLowerInvariant() switch
        {
            "admin" => "Admin",
            "owner" => "Owner",
            _ => "Farmer"
        };
    }
}
