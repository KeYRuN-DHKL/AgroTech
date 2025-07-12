using System.ComponentModel.DataAnnotations;

namespace AgroTechProject.Dtos.ResourceDto;

public class ResourceRequestDto
{   
    [Required(ErrorMessage = "Name is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Name must be between 6 and 100 characters.")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "Description is required.")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "Description must be between 10 and 500 characters.")]
    public string Description { get; set; } = null!;

    [Required(ErrorMessage = "OwnerId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "OwnerId must be a positive number.")]
    public int OwnerId { get; set; }
}
