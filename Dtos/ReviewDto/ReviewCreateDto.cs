namespace AgroTechProject.Dtos.ReviewDto;

public class ReviewCreateDto
{
    public int ResourceId { get; set; }
    public int FarmerId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
}
