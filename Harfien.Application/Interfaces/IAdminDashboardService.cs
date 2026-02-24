using Harfien.Application.DTO.Dashboard;




namespace Harfien.Application.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<Dashboard> GetDashboardSummaryAsync();
        Task<List<OrdersPerDayDto>> GetOrdersPerDayAsync();
        Task<List<UsersByGovernorateDto>> GetUsersByGovernorateAsync();
        Task<List<CraftmanDto>> GetAllCraftmenAsync(string? name);
    }
}
