using Harfien.Application.DTO.Payment;
using Harfien.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces.payment_interfaces
{
    public interface IWalletTransactionService
    {
        Task<WalletTransactionDto> CreateAsync(string userId, CreateWalletTransactionDto dto);
        Task<PagedResult<WalletTransactionDto>> GetMyTransactionsAsync(string userId, int pageNumber, int pageSize);
    }

}
