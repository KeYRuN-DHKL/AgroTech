using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class ResourceModel
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int OwnerId { get; set; }
    public UserModel Owner { get; set; } = null!;

    public ICollection<BookingModel> Bookings { get; set; } = new List<BookingModel>();
}
