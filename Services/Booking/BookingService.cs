using AgroTechProject.Dtos.BookingDto;
using AgroTechProject.Model;
using AgroTechProject.Repositories.BookingRepo;

namespace AgroTechProject.Services.Booking;

// Services/BookingService.cs
public class BookingService : IBookingService
{
    private readonly IBookingRepository _repo;

    public BookingService(IBookingRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync()
    {
        var bookings = await _repo.GetAllAsync();
        return bookings.Select(b => new BookingResponseDto
        {
            Id = b.Id,
            ResourceName = b.Resource != null ? b.Resource.Name : "Unknown Resource",
            UserName = b.User != null ? b.User.FullName : "Unknown User",
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Status = b.Status
        });
    }

    public async Task<BookingResponseDto?> GetBookingByIdAsync(int id)
    {
        var b = await _repo.GetByIdAsync(id);
        return b == null ? null : new BookingResponseDto
        {
            Id = b.Id,
            ResourceName = b.Resource != null ? b.Resource.Name : "Unknown Resource",
            UserName = b.User != null ? b.User.FullName : "Unknown User",
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Status = b.Status
        };
    }

    public async Task<BookingResponseDto> CreateBookingAsync(BookingCreateDto dto)
    {
        var booking = new BookingModel
        {
            ResourceId = dto.ResourceId,
            UserId = dto.UserId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var created = await _repo.CreateAsync(booking);

        return new BookingResponseDto
        {
            Id = created.Id,
            ResourceName = "", // optional: fetch name if needed
            UserName = "",
            StartTime = created.StartTime,
            EndTime = created.EndTime,
            Status = created.Status
        };
    }

    public async Task DeleteBookingAsync(int id) => await _repo.DeleteAsync(id);
}
