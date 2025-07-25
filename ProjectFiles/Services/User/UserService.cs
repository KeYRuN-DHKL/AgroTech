using AgroTechProject.Dtos.UserDto;
using AgroTechProject.Model;
using AgroTechProject.Repositories.UserRepo;

namespace AgroTechProject.Services.User;

public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

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
        var user = new UserModel
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = dto.Password,
            Role = dto.Role,
            PhoneNumber = dto.PhoneNumber
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
        user.PasswordHash = dto.Password;
        user.Role = dto.Role;
        user.PhoneNumber = dto.PhoneNumber;

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
}