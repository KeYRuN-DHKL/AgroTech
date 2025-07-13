using System.Linq.Expressions;
using AgroTechProject.Data;
using AgroTechProject.Enums;
using AgroTechProject.Model;
using Microsoft.EntityFrameworkCore;

namespace AgroTechProject.Repositories.BookingRepo;

public class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<BookingModel>> GetAllAsync() =>
        await _context.Bookings.Include(b => b.Resource).Include(b => b.User).ToListAsync();

    public async Task<BookingModel?> GetByIdAsync(int id) =>
        await _context.Bookings.Include(b => b.Resource).Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == id);

    public async Task<BookingModel> CreateAsync(BookingModel booking)
    {
        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task UpdateAsync(BookingModel booking)
    {
        _context.Bookings.Update(booking);
        await _context.SaveChangesAsync();
    }
    
    public async Task<bool> ExistsAsync(Expression<Func<BookingModel, bool>> predicate)
    {
        return await _context.Bookings.AnyAsync(predicate);
    }

    public async Task DeleteAsync(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking != null)
        {
            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task UpdateStatusAsync(int bookingId, BookingStatus newStatus)
    {
        var booking = await _context.Bookings.FindAsync(bookingId);
        if (booking == null) throw new InvalidOperationException("Booking not found.");
        booking.Status = newStatus;
        await _context.SaveChangesAsync();
    }
}
