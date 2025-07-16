using System.ComponentModel.DataAnnotations;
namespace AgroTechProject.Dtos.BookingDto;

public class BookingCreateDto
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "ResourceId must be greater than 0.")]
    public int ResourceId { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "UserId must be greater than 0.")]
    public int UserId { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; }

    [Required]
    [DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; }
}