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
    public class WalletRepository
    : GenericRepository<Wallet>, IWalletRepository
    {
        public WalletRepository(HarfienDbContext context)
            : base(context)
        {
        }

        public async Task<Wallet?> GetByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(w => w.Transactions)
                .FirstOrDefaultAsync(w => w.UserId == userId);
        }
        public IQueryable<Payment> GetPaymentsByClientId(string clientId)
        {
            return _context.Payments
                .Where(p => p.Order.Client.UserId==clientId)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Service)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Craftsman)
                        .ThenInclude(c => c.User);
        }




        public async Task<bool> HasSufficientBalanceAsync(string userId, decimal amount)
        {
            var wallet = await _dbSet
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wallet == null) return false;

            return wallet.Balance >= amount;
        }
        public Task<Wallet?> GetByUserIdWithTransactionsAsync(string userId)
        {
            var wallet =  _dbSet
                     .Include(w => w.Transactions)
                         .ThenInclude(t => t.Order)
                             .ThenInclude(o => o.Craftsman)
                             .ThenInclude(u=>u.User)
                     .Include(w => w.Transactions)
                         .ThenInclude(t => t.Order)
                             .ThenInclude(o => o.Service)
                     .FirstOrDefaultAsync(w => w.UserId == userId);
            return wallet;
        }
       
    }

}
