using AgroTechProject.Dtos.DashboardDto;

namespace AgroTechProject.Repositories.DashboardRepo
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDto> GetDashboardStatsAsync();
    }
}
