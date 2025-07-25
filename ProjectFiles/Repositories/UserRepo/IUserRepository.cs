using AgroTechProject.Model;

namespace AgroTechProject.Repositories.UserRepo;

public interface IUserRepository
{
    Task<IEnumerable<UserModel>> GetAllAsync();
    Task<UserModel?> GetByIdAsync(int id);
    Task<IEnumerable<UserModel>> SearchByFullNameAsync(string name);
    Task<UserModel?> GetByEmailAsync(string email);
    Task AddAsync(UserModel user);
    void Update(UserModel user);
    void Delete(UserModel user);
    Task SaveChangesAsync();
}
