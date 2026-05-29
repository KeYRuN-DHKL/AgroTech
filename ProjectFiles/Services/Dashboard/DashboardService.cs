using AgroTechProject.Dtos.DashboardDto;
using AgroTechProject.Repositories.DashboardRepo;

namespace AgroTechProject.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repository;

        public DashboardService(IDashboardRepository repository)
        {
            _repository = repository;
        }

        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            return await _repository.GetDashboardStatsAsync();
        }
    }
}
