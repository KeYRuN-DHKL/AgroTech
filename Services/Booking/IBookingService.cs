using AgroTechProject.Dtos.BookingDto;

namespace AgroTechProject.Services.Booking;

public interface IBookingService
{
    Task<IEnumerable<BookingResponseDto>> GetAllBookingsAsync();
    Task<BookingResponseDto?> GetBookingByIdAsync(int id);
    Task<BookingResponseDto> CreateBookingAsync(BookingCreateDto dto);
    Task DeleteBookingAsync(int id);
}