using Harfien.Application.DTO.Dashboard;
using Harfien.Application.Interfaces;
using Harfien.DataAccess;
using Harfien.Domain.Enums;
using Microsoft.EntityFrameworkCore;




namespace Harfien.Infrastructure.Services
{
    public  class AdminDashboardService : IAdminDashboardService
    {
        private readonly HarfienDbContext _context;

        public AdminDashboardService(HarfienDbContext context)
        {
            _context = context;
        }

        public async Task<Dashboard> GetDashboardSummaryAsync()
        {
            var totalCraftmen = await _context.Craftsmen.CountAsync();
            var totalUsers = await _context.Users.CountAsync();
            var totalServices = await _context.Services.CountAsync();

            var completedOrders = await _context.Orders
                 .Where(o => o.Status == OrderStatus.Completed)
                     .ToListAsync();

            var totalProfit = completedOrders.Sum(o => o.Amount * 0.1m); // 10%

            return new Dashboard
            {
                TotalCraftmen = totalCraftmen,
                TotalUsers = totalUsers,
                TotalServices = totalServices,
                CompletedOrdersCount = completedOrders.Count,
                TotalProfit = totalProfit
            };
        }

        public async Task<List<OrdersPerDayDto>> GetOrdersPerDayAsync()
        {
            return await _context.Orders
                .Where(o => o.Status == OrderStatus.Completed )
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new OrdersPerDayDto
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .OrderBy(x => x.Date)
                .ToListAsync();
        }

        public async Task<List<UsersByGovernorateDto>> GetUsersByGovernorateAsync()
        {
            var totalUsers = await _context.Users.CountAsync();

            return await _context.Users
                .GroupBy(u => u.Address)
                .Select(g => new UsersByGovernorateDto
                {
                    City = g.Key,
                    Percentage = (double)g.Count() / totalUsers * 100
                })
                .ToListAsync();
        }

        public async Task<List<CraftmanDto>> GetAllCraftmenAsync(string? name)
        {
            var query = _context.Craftsmen
                .Include(c => c.User)
                .AsQueryable();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(c => c.User.UserName.Contains(name));
            }

            return await query.Select(c => new CraftmanDto
            {
                Id = c.Id,
                Name = c.User.UserName,
                Phone = c.User.PhoneNumber,
                Rating = c.Rating,
                IsVerified = c.IsVerified
            }).ToListAsync();
        }
    }
}
