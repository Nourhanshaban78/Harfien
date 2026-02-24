using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Infrastructure.Repositories
{
    public class SubscriptionPlanDetailsRepository
      : GenericRepository<SubscriptionPlanDetails>,
        ISubscriptionPlanDetailsRepository
    {
        public SubscriptionPlanDetailsRepository(HarfienDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<SubscriptionPlanDetails>> GetByPlanIdAsync(int planId)
        {
            return await _dbSet
                .Where(d => d.SubscriptionPlanId == planId)
                .ToListAsync();
        }

        public async Task DeleteByPlanIdAsync(int planId)
        {
            var details = await _dbSet
                .Where(d => d.SubscriptionPlanId == planId)
                .ToListAsync();

            _dbSet.RemoveRange(details);
        }
    }

}
