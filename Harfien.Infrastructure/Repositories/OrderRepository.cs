using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
       
            public OrderRepository(HarfienDbContext context) : base(context) { }

            public async Task<Order?> GetByIdWithDetailsAsync(int id)
                => await _context.Orders
                    .Include(o => o.Client)
                        .ThenInclude(c => c.User)
                    .Include(o => o.Craftsman)
                    .Include(o => o.Service)
                    .Include(o => o.Payment)
                    .FirstOrDefaultAsync(o => o.Id == id);
            public async Task<IEnumerable<Order>> GetAllWithDetailsAsync()
            {
                return await _context.Orders
                    .Include(o => o.Service)
                    .Include(o => o.Client).ThenInclude(c => c.User)
                    .Include(o => o.Craftsman).ThenInclude(c => c.User)
                    .Include(o => o.Payment)
                    .ToListAsync();
            }


        public async Task<IEnumerable<Order>> GetByClientIdAsync(int clientId)
                => await _context.Orders
                    .Where(o => o.ClientId == clientId)
                    .ToListAsync();

            public async Task<IEnumerable<Order>> GetByCraftsmanIdAsync(int craftsmanId)
                => await _context.Orders
                    .Where(o => o.CraftsmanId == craftsmanId)
                    .ToListAsync();

            public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
                => await _context.Orders
                    .Where(o => o.Status == status)
                    .ToListAsync();
        public async Task<bool> ExistsAsync(int craftsmanId, DateTime scheduledAt)
        {
            return await _dbSet.AnyAsync(o =>
                o.CraftsmanId == craftsmanId &&
                o.ScheduledAt == scheduledAt
            );
        }
    }
    
}
