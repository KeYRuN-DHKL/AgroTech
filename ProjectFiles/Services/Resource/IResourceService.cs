using AgroTechProject.Dtos.ResourceDto;

namespace AgroTechProject.Services.Resource;

public interface IResourceService
{
    Task<IEnumerable<ResourceResponseDto>> GetAllAsync();
    Task<ResourceResponseDto?> GetByIdAsync(int id);
    Task AddAsync(ResourceRequestDto dto);
    Task UpdateAsync(int id, ResourceRequestDto dto);
    Task DeleteAsync(int id);
}