using System.ComponentModel.DataAnnotations;

namespace AgroTechProject.Dtos.ReviewDto;

public class ReviewCreateDto
{
    [Required(ErrorMessage = "ResourceId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ResourceId must be a positive number.")]
    public int ResourceId { get; set; }

    [Required(ErrorMessage = "FarmerId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "FarmerId must be a positive number.")]
    public int FarmerId { get; set; }

    [Required(ErrorMessage = "Rating is required.")]
    [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
    public int Rating { get; set; }

    [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
    public string? Comment { get; set; }
}
