using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using Harfien.Domain.Interface_Repository.Repositories;


namespace Harfien.Infrastructure.Repositories
{
    public class CraftsmanRepository
     : GenericRepository<Craftsman>, ICraftsmanRepository
    {
        public CraftsmanRepository(HarfienDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Craftsman>> GetAllWithUserAsync()
        {
            return await _context.Craftsmen
                .Include(c => c.User)
                .ToListAsync();
        }

        public async Task<Craftsman?> GetByUserIdAsync(string userId)
        {
            return await _context.Craftsmen
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task UpdateAsync(Craftsman craftsman)
        {
            _context.Craftsmen.Update(craftsman);
            await _context.SaveChangesAsync();
        }

        public async Task<Craftsman?> GetByUserIdWithIncludeAsync(string userId)
        {
            return await _context.Craftsmen
                .Include(c => c.User)
                .Include(c => c.CraftsmanServices)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }


        public async Task<Craftsman?> GetProfileAsync(int id)
        {
            return await _context.Craftsmen
             .Include(c => c.User)
             .Include(c => c.CraftsmanServices)
             .Include(c => c.Availabilities)
             .Include(c => c.Orders)
             .FirstOrDefaultAsync(c => c.Id == id && c.IsApproved);
        }

        public async Task<List<Review>?> GetReviewAsync(int craftmanid)
        {
            return await _context.Reviews
             .Where(r => r.CraftsmanId == craftmanid)
             .ToListAsync();
        }


    }

}
