using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgroTechProject.Enums;

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
        
        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending; // Pending, Approved, Rejected
    }
}