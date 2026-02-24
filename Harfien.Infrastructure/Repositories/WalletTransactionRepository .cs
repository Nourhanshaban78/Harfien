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
    public class WalletTransactionRepository
    : GenericRepository<WalletTransaction>, IWalletTransactionRepository
    {
        public WalletTransactionRepository(HarfienDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<WalletTransaction>> GetByWalletIdAsync(int walletId)
        {
            return await _dbSet
                .Where(t => t.WalletId == walletId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<WalletTransaction>> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .Where(t => t.OrderId == orderId)
                .ToListAsync();
        }

        public async Task<List<WalletTransaction>> GetTransactionsByUserIdAsync(string userId, int skip, int take)
        {
                    return await _dbSet
                .Where(t => t.Wallet.UserId == userId)
                .Include(t => t.Order)
                    .ThenInclude(o => o.Service)
                .Include(t => t.Order)
                    .ThenInclude(o => o.Craftsman)
                        .ThenInclude(u=>u.User)
                .OrderByDescending(t => t.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();
        }

        public async Task<int> GetTransactionsCountByUserIdAsync(string userId)
        {
            return await _dbSet .CountAsync(t => t.Wallet.UserId == userId);
        }

    }

}
