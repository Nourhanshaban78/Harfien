using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Infrastructure.Repositories
{
    public class CityRepository : ICityRepository
    {
        private readonly HarfienDbContext _context;
        private readonly DbSet<City> _dbSet;
        public CityRepository(HarfienDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<City>();
        }
        public async Task<IEnumerable<City>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<City?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
    }
}