using AgroTechProject.Data;
using AgroTechProject.Model;

namespace AgroTechProject.Repositories.UserRepo;

using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<UserModel>> GetAllAsync() =>
        await _context.Users.ToListAsync();

    public async Task<UserModel?> GetByIdAsync(int id) =>
        await _context.Users.FindAsync(id);

    public async Task AddAsync(UserModel user) =>
        await _context.Users.AddAsync(user);

    public void Update(UserModel user) =>
        _context.Users.Update(user);

    public void Delete(UserModel user) =>
        _context.Users.Remove(user);

    public async Task SaveChangesAsync() =>
        await _context.SaveChangesAsync();
}
