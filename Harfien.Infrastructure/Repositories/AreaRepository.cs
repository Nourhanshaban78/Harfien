using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class AreaRepository : IAreaRepository
    {
        private readonly HarfienDbContext _context;
        private readonly DbSet<Area> _dbSet;
        public AreaRepository(HarfienDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<Area>();
        }
        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Area>> GetAllByCityIdAsync(int cityId)
        {
            return await _dbSet.Where(a => a.CityId == cityId).ToListAsync();
        }

        public async Task<Area?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}