namespace AgroTechProject.Dtos.ResourceDto;

public class ResourceRequestDto
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public int OwnerId { get; set; }
}
