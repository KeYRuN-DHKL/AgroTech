namespace AgroTechProject.Dtos.ResourceDto;

public class ResourceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required string Description { get; set; } = null!;
    public int OwnerId { get; set; }
    public required string OwnerName { get; set; } = null!;
}