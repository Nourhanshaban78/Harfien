using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class ServiceCategoryRepository
        : GenericRepository<ServiceCategory>, IServiceCategoryRepository
    {
        private readonly HarfienDbContext _context;

        public ServiceCategoryRepository(HarfienDbContext context)
            : base(context)
        {
            _context = context;
        }

      
        public async Task<ServiceCategory?> GetServiceCategoryByServiceIdAsync(int id)
        {
            return await _context.Services
                .Where(s => s.Id == id)
                .Select(s => s.ServiceCategory)
                .FirstOrDefaultAsync();
        }

    
        public async Task<ServiceCategory?> GetByNameAsync(string name)
        {
            return await _context.ServiceCategories
                .FirstOrDefaultAsync(c => c.Name == name);
        }

        
        public async Task<ServiceCategory?> GetWithServicesAsync(int id)
        {
            return await _context.ServiceCategories
                .Include(c => c.Services)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

      
        public async Task<bool> HasServicesAsync(int id)
        {
            return await _context.Services
                .AnyAsync(s => s.ServiceCategoryId == id);
        }
    }
}
