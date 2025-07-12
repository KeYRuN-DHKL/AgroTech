using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class ResourceModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int OwnerId { get; set; }

    [ForeignKey("OwnerId")]
    public UserModel? Owner { get; set; }

    public ICollection<BookingModel> Bookings { get; set; } = new List<BookingModel>();
}
