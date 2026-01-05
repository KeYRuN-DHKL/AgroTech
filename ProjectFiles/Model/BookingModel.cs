using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AgroTechProject.Enums;

namespace AgroTechProject.Model
{
    public class BookingModel
    {
        public int Id { get; set; }

        public int ResourceId { get; set; }
        public virtual ResourceModel Resource { get; set; } = null!;

        public int UserId { get; set; }
        public virtual UserModel User { get; set; } = null!;

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        
        [Required]
        public BookingStatus Status { get; set; } = BookingStatus.Pending; // Pending, Approved, Rejected
    }
}