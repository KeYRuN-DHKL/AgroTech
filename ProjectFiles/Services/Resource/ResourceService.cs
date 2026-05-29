using AgroTechProject.Dtos.ResourceDto;
using AgroTechProject.Model;
using AgroTechProject.Repositories.ResourceRepo;
using AgroTechProject.Settings;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace AgroTechProject.Services.Resource;

public class ResourceService : IResourceService
{
    private readonly IResourceRepository _repo;
    private readonly Cloudinary _cloudinary;
    private readonly ILogger<ResourceService> _logger;

    public ResourceService(IResourceRepository repo, IConfiguration configuration, ILogger<ResourceService> logger)
    {
        _repo = repo;
        _logger = logger;

        var settings = new CloudinarySettings
        {
            CloudName = configuration["CloudinarySettings:CloudName"] ??
                   configuration["CloudinarySettings__CloudName"] ?? string.Empty,

            ApiKey = configuration["CloudinarySettings:ApiKey"] ??
                configuration["CloudinarySettings__ApiKey"] ?? string.Empty,

            ApiSecret = configuration["CloudinarySettings:ApiSecret"] ??
                   configuration["CloudinarySettings__ApiSecret"] ?? string.Empty
        };

        if (string.IsNullOrEmpty(settings.CloudName) ||
            string.IsNullOrEmpty(settings.ApiKey) ||
            string.IsNullOrEmpty(settings.ApiSecret))
        {
            throw new Exception("Cloudinary settings are not configured properly.");
        }


        var account = new Account(settings.CloudName, settings.ApiKey, settings.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }

    public async Task<IEnumerable<ResourceResponseDto>> GetAllAsync()
    {
        var resources = await _repo.GetAllAsync();
        return resources.Select(resource => new ResourceResponseDto
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
            OwnerId = resource.OwnerId,
            OwnerName = resource.Owner.FullName,
            ImageUrl = resource.ImageUrl
        });
    }

    public async Task<ResourceResponseDto?> GetByIdAsync(int id)
    {
        var resource = await _repo.GetByIdAsync(id);
        if (resource == null) return null;

        return new ResourceResponseDto
        {
            Id = resource.Id,
            Name = resource.Name,
            Description = resource.Description,
            OwnerId = resource.OwnerId,
            OwnerName = resource.Owner.FullName,
            ImageUrl = resource.ImageUrl
        };
    }

    public async Task AddAsync(ResourceRequestDto dto)
    {
        var (ImageUrl,ResourceId) = await UploadImageAsync(dto.Image);

        var resource = new ResourceModel
        {
            Name = dto.Name,
            Description = dto.Description,
            OwnerId = dto.OwnerId,
            ImageUrl = ImageUrl!,
            PublicId = ResourceId,
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(resource);
    }

    public async Task UpdateAsync(int id,ResourceRequestDto dto)
    {
        var resource = await _repo.GetByIdAsync(id);
        if (resource == null)
            throw new KeyNotFoundException("Resource Not Found...");

        if(dto.Image != null)
        {
            await DeleteImageFromCloudinary(resource.PublicId);
            var (newImageUrl, newPublicId) = await UploadImageAsync(dto.Image);
            resource.ImageUrl = newImageUrl!;
            resource.PublicId = newPublicId;
        }

        resource.Name = dto.Name;
        resource.Description = dto.Description;

        await _repo.UpdateAsync(resource);
    }

    public async Task DeleteAsync(int id)
    {
        var resource = await _repo.GetByIdAsync(id);
        if (resource != null)
        {
            await DeleteImageFromCloudinary(resource.PublicId);
            await _repo.DeleteAsync(resource);
        }
    }

    private async Task<(string? imageUrl,string? publicId)> UploadImageAsync(IFormFile? file)
    {
        if(file == null || file.Length == 0)
        {
            return (null,null);
        }

        ValidateImage(file);

        await using var stream = file.OpenReadStream();

        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(file.FileName, stream),
            Transformation = new Transformation().Width(800).Height(800).Crop("limit")
        };

        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
        return (uploadResult?.SecureUrl?.ToString(),uploadResult?.PublicId);
    }

    private void ValidateImage(IFormFile file)
    {
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (!allowedExtensions.Contains(extension))
            throw new InvalidOperationException("File extension not supported...");

        if (file.Length > 5 * 1024 * 1024)
            throw new InvalidOperationException("File size should be less than 5MB...");
    }

    private async Task DeleteImageFromCloudinary(string? publicId)
    {
        if (string.IsNullOrWhiteSpace(publicId))
        {
            throw new KeyNotFoundException("The Image not available...");
        }

        try
        {
            var deleteParams = new DeletionParams(publicId);
            await _cloudinary.DestroyAsync(deleteParams);
        }catch(Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }
}