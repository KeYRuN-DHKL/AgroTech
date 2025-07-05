using System.ComponentModel.DataAnnotations.Schema;
namespace AgroTechProject.Model;

public class ReviewModel
{
    public int Id { get; set; }

    public int ResourceId { get; set; }

    [ForeignKey("ResourceId")]
    public ResourceModel Resource { get; set; }

    public int FarmerId { get; set; }

    [ForeignKey("FarmerId")]
    public UserModel Farmer { get; set; }

    public int Rating { get; set; }

    public string Comment { get; set; }
}