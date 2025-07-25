using AgroTechProject.Dtos.BookingDto;
using AgroTechProject.Dtos.DashBoardDto;
using AgroTechProject.Enums;
using AgroTechProject.Model;
using AgroTechProject.Repositories.BookingRepo;

namespace AgroTechProject.Services.Booking;

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
        var isOverlapping = await _repo.ExistsAsync(b =>
            b.ResourceId == dto.ResourceId &&
            b.Status != BookingStatus.Rejected &&
            b.StartTime < dto.EndTime &&
            dto.StartTime < b.EndTime
        );

        if (isOverlapping)
        {
            throw new InvalidOperationException(
                "The selected time slot overlaps with an existing booking. Please choose a different time."
            );
        }

        var booking = new BookingModel
        {
            ResourceId = dto.ResourceId,
            UserId = dto.UserId,
            StartTime = dto.StartTime,
            EndTime = dto.EndTime
        };

        var created = await _repo.CreateAsync(booking);
        var full = await _repo.GetByIdAsync(created.Id); 

        return new BookingResponseDto
        {
            Id = created.Id,
            ResourceName = full?.Resource?.Name ?? "Unknown",
            UserName = full?.User?.FullName ?? "Unknown",
            StartTime = created.StartTime,
            EndTime = created.EndTime,
            Status = created.Status
        };
    }
    
    public async Task<IEnumerable<BookingPendingResponseDto>> GetPendingBookingsAsync()
    {
        var pendingBookings = await _repo.GetAllPendingBookingsAsync();

        return pendingBookings.Select(b => new BookingPendingResponseDto
        {
            BookingId = b.Id,
            FarmerId = b.UserId,
            FarmerName = b.User?.FullName ?? "Unknown",
            StartTime = b.StartTime,
            EndTime = b.EndTime,
            Status = b.Status.ToString()
        });
    }
    
    public async Task<bool> UpdateBookingStatusAsync(BookingStatusUpdateDto dto)
    {
        var booking = await _repo.GetByIdWithUserAsync(dto.BookingId);
        if (booking == null || booking.Status != BookingStatus.Pending)
            return false;
    
        booking.Status = dto.NewStatus;
    
        await _repo.UpdateBookingAsync(booking);
        return true;
    }

    // BookingService.cs
    public async Task<List<AdminBookingOverviewDto>> GetAdminDashboardDataAsync()
    {
        var bookings = await _repo.GetAllBookingsWithRelationsAsync();

        return bookings.Select(b => new AdminBookingOverviewDto
        {
            BookingId = b.Id,
            OwnerName = b.Resource?.Owner?.FullName ?? "Unknown Owner",
            ResourceName = b.Resource?.Name ?? "Unknown Resource",
            FarmerName = b.User?.FullName ?? "Unknown Farmer",
            StartTime = b.StartTime,
            EndTime = b.EndTime
        }).ToList();
    }

    public async Task DeleteBookingAsync(int id) => await _repo.DeleteAsync(id);
    
    // public async Task UpdateBookingStatusAsync(int bookingId, BookingStatus status)
    // {
    //     await _repo.UpdateStatusAsync(bookingId, status);
    // }
}
