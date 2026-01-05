using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class ReviewModel
{
    public int Id { get; set; }

    public int ResourceId { get; set; }
    public ResourceModel Resource { get; set; } = null!;

    public int FarmerId { get; set; }
    public UserModel Farmer { get; set; } = null!;

    public int Rating { get; set; }

    public string Comment { get; set; } = null!;
}