using AgroTechProject.Dtos.DashboardDto;

namespace AgroTechProject.Services.Dashboard
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
