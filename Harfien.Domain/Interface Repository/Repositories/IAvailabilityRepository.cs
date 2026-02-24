using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    // Renamed the interface to avoid the CS0101 conflict
    public interface IAvailabilityRepository : IGenericRepository<CraftsmanAvailability>
    {
        Task AddAsync(CraftsmanAvailability availability);
        void Update(CraftsmanAvailability availability);
        Task SaveAsync();

        Task<IEnumerable<CraftsmanAvailability>> GetAllByCraftsmanIdAsync(int craftsmanId);

        Task<bool> IsAvailableAsync(int craftsmanId, DateTime scheduledAt);

        Task<IEnumerable<int>> GetAvailableCraftsmenIdsAsync(DateTime scheduledAt);
    }

}
