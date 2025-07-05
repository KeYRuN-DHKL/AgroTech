using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class UserModel
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    [Required]
    public string Role { get; set; }

    public ICollection<ResourceModel> Resources { get; set; }
    public ICollection<BookingModel> Bookings { get; set; }
}
