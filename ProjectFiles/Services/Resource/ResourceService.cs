using AgroTechProject.Dtos.ResourceDto;
using AgroTechProject.Model;
using AgroTechProject.Repositories.ResourceRepo;

namespace AgroTechProject.Services.Resource;

public class ResourceService : IResourceService
{
    private readonly IResourceRepository _repo;

    public ResourceService(IResourceRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<ResourceResponseDto>> GetAllAsync()
    {
        var resources = await _repo.GetAllAsync();
        return resources.Select(r => new ResourceResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            OwnerId = r.OwnerId,
            OwnerName = r.Owner.FullName
        });
    }

    public async Task<ResourceResponseDto?> GetByIdAsync(int id)
    {
        var r = await _repo.GetByIdAsync(id);
        if (r == null) return null;

        return new ResourceResponseDto
        {
            Id = r.Id,
            Name = r.Name,
            Description = r.Description,
            OwnerId = r.OwnerId,
            OwnerName = r.Owner.FullName
        };
    }

    public async Task AddAsync(ResourceRequestDto dto)
    {
        var resource = new ResourceModel
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId
        };

        await _repo.AddAsync(resource);
    }

    public async Task UpdateAsync(int id, ResourceRequestDto dto)
    {
        var resource = await _repo.GetByIdAsync(id);
        if (resource == null) return;

        resource.Name = dto.Name;
        resource.Description = dto.Description;
        resource.OwnerId = dto.OwnerId;

        await _repo.UpdateAsync(resource);
    }

    public async Task DeleteAsync(int id)
    {
        var resource = await _repo.GetByIdAsync(id);
        if (resource != null)
        {
            await _repo.DeleteAsync(resource);
        }
    }
}