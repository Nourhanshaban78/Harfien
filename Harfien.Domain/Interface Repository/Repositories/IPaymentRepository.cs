using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<Payment?> GetByOrderIdAsync(int orderId);
        Task<Payment?> GetByIdWithOrderAsync(int paymentId);
        Task<IEnumerable<Payment>> GetByStatusAsync(PaymentStatus status);
    }

}
