namespace AgroTechProject.Dtos.ResourceDto;

public class ResourceResponseDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int OwnerId { get; set; }
    public required string OwnerName { get; set; }
}