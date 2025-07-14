using AgroTechProject.Dtos.BookingDto;
using AgroTechProject.Dtos.DashBoardDto;
using AgroTechProject.Enums;

namespace AgroTechProject.Services.Booking;

public interface IBookingService
{
    Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
    Task<BookingResponseDto?> GetBookingByIdAsync(int id);
    Task<BookingResponseDto> CreateBookingAsync(BookingCreateDto dto);
    Task DeleteBookingAsync(int id);
    // Task UpdateBookingStatusAsync(int bookingId, BookingStatus status);
    Task<IEnumerable<BookingPendingResponseDto>> GetPendingBookingsAsync();
    Task<bool> UpdateBookingStatusAsync(BookingStatusUpdateDto dto);
    Task<List<AdminBookingOverviewDto>> GetAdminDashboardDataAsync();
}