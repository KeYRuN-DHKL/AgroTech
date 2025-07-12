namespace AgroTechProject.Dtos.BookingDto;

public class BookingResponseDto
{
    public int Id { get; set; }
    public required string ResourceName { get; set; } = null!;
    public required string UserName { get; set; } = null!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public required string Status { get; set; } = null!;
}