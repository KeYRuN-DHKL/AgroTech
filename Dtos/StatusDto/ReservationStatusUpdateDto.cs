using AgroTechProject.Enums;
namespace AgroTechProject.Dtos.StatusDto;
public class ReservationStatusUpdateDto
{
    public required BookingStatus Status { get; set; } 
}