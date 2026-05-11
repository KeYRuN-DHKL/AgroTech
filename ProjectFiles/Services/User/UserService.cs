using AgroTechProject.Dtos.UserDto;
using AgroTechProject.Helpers;
using AgroTechProject.Model;
using AgroTechProject.Repositories.UserRepo;
using AgroTechProject.Settings;
using Microsoft.Extensions.Options;

namespace AgroTechProject.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    private readonly JwtSettings _jwtSettings;
    public UserService(IUserRepository repo,IOptions<JwtSettings> jwtSettings)
    {
        _repo = repo;
        _jwtSettings = jwtSettings.Value;

    }

    public async Task<IEnumerable<UserResponseDto>> GetAllAsync()
    {
        var users = await _repo.GetAllAsync();
        return users.Select(u => new UserResponseDto
        {
            Id = u.Id,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role,
            PhoneNumber = u.PhoneNumber
        });
    }

    public async Task<UserResponseDto?> GetByIdAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        return user == null ? null : new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber
        };
    }
    
    public async Task<IEnumerable<UserResponseDto>> SearchByFullNameAsync(string name)
    {
        var users = await _repo.SearchByFullNameAsync(name);

        return users.Select(user => new UserResponseDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            Role = user.Role,
            PhoneNumber = user.PhoneNumber
        });
    }


    public async Task CreateAsync(UserRequestDto dto)
    {
        var refreshToken = TokenGenerator.GenerateRefreshToken();
        var user = new UserModel
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = PasswordHasher.Hash(dto.Password),
            Role = NormalizeRole(dto.Role),
            PhoneNumber = dto.PhoneNumber,
            RefreshToken = refreshToken,
            RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays)
        };

        await _repo.AddAsync(user);
        await _repo.SaveChangesAsync();
    }

    public async Task UpdateAsync(int id, UserRequestDto dto)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return;

        user.FullName = dto.FullName;
        user.Email = dto.Email;
        user.Role = NormalizeRole(dto.Role);
        user.PhoneNumber = dto.PhoneNumber;

        _repo.Update(user);
        await _repo.SaveChangesAsync();
    }

    public async Task ForgotPasswordAsync(string email,string password)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null) return;

        user.PasswordHash = PasswordHasher.Hash(password);
        user.RefreshToken = TokenGenerator.GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenExpiryInDays);

        _repo.Update(user);
        await _repo.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var user = await _repo.GetByIdAsync(id);
        if (user == null) return;

        _repo.Delete(user);
        await _repo.SaveChangesAsync();
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