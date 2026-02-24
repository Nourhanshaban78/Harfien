using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(HarfienDbContext context) : base(context) { }
        public async Task<IEnumerable<Review>> GetAllByCraftsmanIdAsync(int craftsmanId)
        {
            return await _dbSet
                .Include(r => r.Order)
                    .ThenInclude(o => o.Client)
                        .ThenInclude(c => c.User)
                .Where(r => r.CraftsmanId == craftsmanId)
                .ToListAsync();
        }
        public async Task<(IEnumerable<Review> Reviews, int TotalCount)> GetPagedByCraftsmanIdAsync(int craftsmanId, int pageNumber, int pageSize)
        {
            var query = _dbSet.Where(r => r.CraftsmanId == craftsmanId);

            int totalCount = await query.CountAsync();

            var reviews = await query
                .Include(r => r.Order)
                    .ThenInclude(o => o.Client)
                        .ThenInclude(c => c.User)
                .OrderByDescending(r => r.CreatedAt) // Newest reviews first
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (reviews, totalCount);
        }
        public async Task<bool> HasReviewForOrderAsync(int orderId)
        {
            return await _dbSet.AnyAsync(r => r.OrderId == orderId);
        }
    }
}