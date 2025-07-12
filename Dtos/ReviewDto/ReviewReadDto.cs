namespace AgroTechProject.Dtos.ReviewDto;

public class ReviewReadDto
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public string? ResourceName { get; set; } = null!;
    public int FarmerId { get; set; }
    public string? FarmerName { get; set; } = null!;
    public int Rating { get; set; }
    public string? Comment { get; set; } = null!;
}
