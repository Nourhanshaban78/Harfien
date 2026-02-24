using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IServiceCategoryRepository : IGenericRepository<ServiceCategory>
    {
        
        Task<ServiceCategory?> GetServiceCategoryByServiceIdAsync(int Id);
        Task<ServiceCategory?> GetByNameAsync(string name);
        Task<ServiceCategory?> GetWithServicesAsync(int Id);
        Task<bool> HasServicesAsync(int Id); 

    }
}
