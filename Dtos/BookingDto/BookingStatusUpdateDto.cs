using AgroTechProject.Enums;

namespace AgroTechProject.Dtos.BookingDto
{
    public class BookingStatusUpdateDto
    {
        public int BookingId { get; set; }
        public BookingStatus NewStatus { get; set; }
    }
}