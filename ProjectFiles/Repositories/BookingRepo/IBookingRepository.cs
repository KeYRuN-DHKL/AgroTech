using System.Linq.Expressions;
using AgroTechProject.Enums;
using AgroTechProject.Model;
namespace AgroTechProject.Repositories.BookingRepo;

public interface IBookingRepository
{
    Task<IEnumerable<BookingModel>> GetAllAsync();
    Task<BookingModel?> GetByIdAsync(int id);
    Task<BookingModel> CreateAsync(BookingModel booking);
    Task UpdateAsync(BookingModel booking);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(Expression<Func<BookingModel, bool>> predicate);
    // Task UpdateStatusAsync(int bookingId, BookingStatus newStatus);
    Task<BookingModel?> FindAsync(int id);
    Task<IEnumerable<BookingModel>> GetAllPendingBookingsAsync();
    Task<BookingModel?> GetByIdWithUserAsync(int id);
    Task UpdateBookingAsync(BookingModel booking);
    Task<List<BookingModel>> GetAllBookingsWithRelationsAsync();
}