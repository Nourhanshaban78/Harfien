using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IWalletTransactionRepository
      : IGenericRepository<WalletTransaction>
    {
        Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(int walletId);
        Task<IEnumerable<WalletTransaction>> GetByOrderIdAsync(int orderId);
        Task<List<WalletTransaction>> GetTransactionsByUserIdAsync(string userId, int skip, int take);
        Task<int> GetTransactionsCountByUserIdAsync(string userId);
    }

}
