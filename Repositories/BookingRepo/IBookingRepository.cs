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
    Task UpdateStatusAsync(int bookingId, BookingStatus newStatus);
}