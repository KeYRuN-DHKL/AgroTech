using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class ResourceModel
{
    public int Id { get; set; }

    [StringLength(30, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 30 characters.")]
    [Required]
    public required string Name { get; set; }

    [StringLength(50, MinimumLength = 3, ErrorMessage = "Description must be between 3 and 50 characters.")]
    [Required]
    public string? Description { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    public  UserModel? Owner { get; set; }

    public ICollection<BookingModel> Bookings { get; set; } =new List<BookingModel>();
}
