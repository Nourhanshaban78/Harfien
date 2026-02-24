using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface ISubscriptionPlanDetailsRepository
    : IGenericRepository<SubscriptionPlanDetails>
    {
        Task<IEnumerable<SubscriptionPlanDetails>> GetByPlanIdAsync(int planId);
        Task DeleteByPlanIdAsync(int planId);
    }

}
