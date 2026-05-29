namespace AgroTechProject.Dtos.DashboardDto
{
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalFarmers { get; set; }
        public int TotalOwners { get; set; }

        public int TotalResources { get; set; }

        public int TotalBookings { get; set; }

        public int PendingBookings { get; set; }
        public int ApprovedBookings { get; set; }
        public int RejectedBookings { get; set; }

        public List<MonthlyUserTrendDto> UserGrowth { get; set; } = new();

        public List<MonthlyResourceTrendDto> ResourceGrowth { get; set; } = new();

        public List<MonthlyBookingTrendDto> BookingGrowth { get; set; } = new();
    }
}
