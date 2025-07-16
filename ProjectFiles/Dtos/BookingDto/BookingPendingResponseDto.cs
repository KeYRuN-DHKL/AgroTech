namespace AgroTechProject.Dtos.BookingDto;

public class BookingPendingResponseDto
{
    public int BookingId { get; set; }
    public int FarmerId { get; set; }
    public string FarmerName { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Status { get; set; } = string.Empty; 
}