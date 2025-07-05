using System.ComponentModel.DataAnnotations;
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

    [StringLength(50, MinimumLength = 5, ErrorMessage = "Comment must be between 5 and 50 characters.")]
    public string? Comment { get; set; }
}