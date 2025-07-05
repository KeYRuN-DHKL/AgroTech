using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;
public class BookingModel
{
    public int Id { get; set; }

    [Required]
    public int ResourceId { get; set; }

    [ForeignKey("ResourceId")]
    public ResourceModel Resource { get; set; }

    [Required]
    public int FarmerId { get; set; }

    [ForeignKey("FarmerId")]
    public UserModel Farmer { get; set; }

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    [Required]
    public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
}