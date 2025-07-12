using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgroTechProject.Model
{
    public class BookingModel
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }

        [ForeignKey("ResourceId")]
        public virtual ResourceModel? Resource { get; set; }

        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual UserModel? User { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        [StringLength(10, MinimumLength = 3, ErrorMessage = "status must be between 3 and 10 characters.")]
        [Required]
        public string Status { get; set; } = "Pending"; // Pending, Approved, Rejected
    }
}