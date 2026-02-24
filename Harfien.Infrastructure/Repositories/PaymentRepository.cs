using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using Harfien.Domain.Shared.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Infrastructure.Repositories
{
    public class PaymentRepository: GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(HarfienDbContext context) : base(context) { }

        public async Task<Payment?> GetByOrderIdAsync(int orderId)
        {
            return await _dbSet
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
        }

        public async Task<Payment?> GetByIdWithOrderAsync(int paymentId)
        {
            return await _dbSet
                .Include(p => p.Order)
                    .ThenInclude(o => o.Client)
                .Include(p => p.Order)
                    .ThenInclude(o => o.Craftsman)
                .FirstOrDefaultAsync(p => p.Id == paymentId);
        }

        public async Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status)
        {
            return await _dbSet
                .Include(p => p.Order)
                .Where(p => p.Status == status)
                .ToListAsync();
        }
    }

}
