using AgroTechProject.Model;

namespace AgroTechProject.Repositories.ResourceRepo;

public interface IResourceRepository
{
    Task<IEnumerable<ResourceModel>> GetAllAsync();
    Task<ResourceModel?> GetByIdAsync(int id);
    Task AddAsync(ResourceModel resource);
    Task UpdateAsync(ResourceModel resource);
    Task DeleteAsync(ResourceModel resource);
}