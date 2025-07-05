using AgroTechProject.Dtos.UserDto;

namespace AgroTechProject.Services.User;

using AgroTechProject.Dtos.UserDto;

public interface IUserService
{
    Task<IEnumerable<UserResponseDto>> GetAllAsync();
    Task<UserResponseDto?> GetByIdAsync(int id);
    Task CreateAsync(UserRequestDto dto);
    Task UpdateAsync(int id, UserRequestDto dto);
    Task DeleteAsync(int id);
}
