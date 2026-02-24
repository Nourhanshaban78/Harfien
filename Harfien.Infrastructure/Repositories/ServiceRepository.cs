using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(HarfienDbContext context) : base(context) { }

      


        public async Task<List<Service>> GetAllServicesWithCraftData()
        {
            return await _dbSet
                .Include(s => s.Craftsman)
                      .ThenInclude(c => c.User)
                      .ThenInclude(s => s.Area)
                 .Include(s => s.ServiceCategory)
                 .ToListAsync();
        }

        
        public async Task<Service?> GetServiceByIdWithCraftData(int id)
        {
              return await _dbSet
                    .Include(s => s.Craftsman)
                         .ThenInclude(c => c.User)
                            .ThenInclude(s=>s.Area)
                    .Include(s => s.ServiceCategory)
                    .FirstOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Service>> GetServicesByCategory(int id)
        {
           return await _dbSet.Where(s=>s.ServiceCategoryId== id)
                 .Include(s => s.Craftsman)
                     .ThenInclude(c => c.User)
                     .ThenInclude(s => s.Area)
                .Include(s => s.ServiceCategory)
                .ToListAsync();
        }


        public async Task<PagedResult<Service>> GetFilteredAsync(ServiceQueryDto query)
        {
            var services = _context.Services
                .Include(s => s.ServiceCategory)
                .Include(s => s.Craftsman)
                    .ThenInclude(c => c.User)
                    .ThenInclude(s => s.Area)
                .AsQueryable();

            
            if (query.CategoryId.HasValue)
            {
                services = services.Where(s => s.ServiceCategoryId == query.CategoryId);
            }
            if (query.CraftnamId.HasValue)
            {
                services = services.Where(s => s.CraftsmanId == query.CraftnamId);
            }

            if (!string.IsNullOrEmpty(query.Area))
            {
                services = services.Where(s => s.Craftsman.User.Area.Name.ToLower() == query.Area.ToLower());
            }

            if (!string.IsNullOrEmpty(query.Search))
            {
                services = services.Where(s =>
                    s.Name.Contains(query.Search) ||
                    s.Description.Contains(query.Search));
            }

            var totalCount = await services.CountAsync();

            var items = await services
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync();

            return new PagedResult<Service>
            {
                Items = items,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize
            };
        }

        public async Task<PagedResult<Service>> GetServicesByCraftsmanIdAsync(
            int craftsmanId,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Services
                .Include(s => s.ServiceCategory)
                .Include(s => s.Craftsman)
                    .ThenInclude(c => c.User)
                    .ThenInclude(u => u.Area)
                .Where(s => s.CraftsmanId == craftsmanId)
                .AsQueryable();

            var totalCount = await query.CountAsync();

            var services = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                
                .ToListAsync();

            return new PagedResult<Service>
            {
                Items = services,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}