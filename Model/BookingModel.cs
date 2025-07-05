using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroTechProject.Model
{
    public class BookingModel
    {
        public int Id { get; set; }

        [Required]
        public int ResourceId { get; set; }
        
        [ForeignKey("ResourceId")]
        public virtual ResourceModel? Resource { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel? User { get; set; }  // Renamed from UserModel to User

        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }

        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}