using AgroTechProject.Data;
using AgroTechProject.Model;
using Microsoft.EntityFrameworkCore;

namespace AgroTechProject.Repositories.ResourceRepo;

public class ResourceRepository : IResourceRepository
{
    private readonly AppDbContext _context;

    public ResourceRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ResourceModel>> GetAllAsync() =>
        await _context.Resources.Include(r => r.Owner).ToListAsync();

    public async Task<ResourceModel?> GetByIdAsync(int id) =>
        await _context.Resources.Include(r => r.Owner)
            .FirstOrDefaultAsync(r => r.Id == id);

    public async Task AddAsync(ResourceModel resource)
    {
        _context.Resources.Add(resource);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ResourceModel resource)
    {
        _context.Resources.Update(resource);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(ResourceModel resource)
    {
        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync();
    }
}