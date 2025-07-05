namespace AgroTechProject.Dtos.ReviewDto;

public class ReviewReadDto
{
    public int Id { get; set; }
    public int ResourceId { get; set; }
    public string? ResourceName { get; set; }
    public int FarmerId { get; set; }
    public string? FarmerName { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
