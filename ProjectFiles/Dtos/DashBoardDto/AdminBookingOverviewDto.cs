namespace AgroTechProject.Dtos.DashBoardDto;

public class AdminBookingOverviewDto
{
    public int BookingId { get; set; }
    public string OwnerName { get; set; }
    public string ResourceName { get; set; }
    public string FarmerName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}