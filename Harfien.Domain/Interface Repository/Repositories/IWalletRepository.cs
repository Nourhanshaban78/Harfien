using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IWalletRepository : IGenericRepository<Wallet>
    {
        Task<Wallet?> GetByUserIdAsync(string userId);
        Task<bool> HasSufficientBalanceAsync(string userId, decimal amount);
        Task<Wallet?> GetByUserIdWithTransactionsAsync(string userId);
        IQueryable<Payment> GetPaymentsByClientId(string clientId);

    }

}
