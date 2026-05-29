using AgroTechProject.Data;
using AgroTechProject.Dtos.DashboardDto;
using AgroTechProject.Enums;
using AgroTechProject.Repositories.DashboardRepo;
using Microsoft.EntityFrameworkCore;

public class DashboardRepository : IDashboardRepository
{
    private readonly AppDbContext _context;

    public DashboardRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<DashboardStatsDto> GetDashboardStatsAsync()
    {
        var stats = new DashboardStatsDto();

        stats.TotalUsers = await _context.Users.CountAsync();

        stats.TotalFarmers = await _context.Users
            .CountAsync(x => x.Role == "Farmer");

        stats.TotalOwners = await _context.Users
            .CountAsync(x => x.Role == "Owner");

        stats.TotalResources = await _context.Resources.CountAsync();

        stats.TotalBookings = await _context.Bookings.CountAsync();

        stats.PendingBookings = await _context.Bookings
            .CountAsync(x => x.Status == BookingStatus.Pending);

        stats.ApprovedBookings = await _context.Bookings
            .CountAsync(x => x.Status == BookingStatus.Approved);

        stats.RejectedBookings = await _context.Bookings
            .CountAsync(x => x.Status == BookingStatus.Rejected);

        return stats;
    }
}