using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IClientRepository : IGenericRepository<Client>
    {
        Task<Client?> GetClientWithOrdersAsync(int clientId);
        Task<Client?> GetByUserIdAsync(string userId);
    }
}
