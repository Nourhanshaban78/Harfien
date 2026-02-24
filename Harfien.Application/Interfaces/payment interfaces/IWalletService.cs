using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Application.DTO.Payment;
using Harfien.Domain.Shared;

namespace Harfien.Application.Interfaces.payment_interfaces
{
    public interface IWalletService
    {
        Task<WalletDto> GetWalletByUserIdAsync(string userId);
        Task<WalletDto> CreateWalletAsync(string userId);
     
        Task<decimal> GetBalanceAsync(string userId);

        Task<bool> DeleteWalletAsync(string userId);
        Task<PagedResult<WalletTransactionDto>> GetTransactionsAsync(
            string userId, int pageNumber, int pageSize);
      Task<PagedResult<ClientPaymentDto>> GetPaymentsByClientIdAsync(
   string clientuserId, int pageNumber, int pageSize);
    }
}
