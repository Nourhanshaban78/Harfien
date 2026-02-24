using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Harfien.Domain.Shared.Repositories
{
    public interface ICraftsmanRepository
        : IGenericRepository<Craftsman>
    {
        Task<IEnumerable<Craftsman>> GetAllWithUserAsync();
        Task<Craftsman?> GetByUserIdAsync(string userId);
        Task UpdateAsync(Craftsman craftsman);
        Task<Craftsman?> GetByUserIdWithIncludeAsync(string userId);
        Task<Craftsman?> GetProfileAsync(int id);
        Task<List<Review>?> GetReviewAsync(int craftmanid);
       
    }

}
