using Harfien.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared.Repositories
{
    public interface IUnitOfWork
    {
        Task SaveAsync();
        IServiceCategoryRepository ServiceCategories { get; }
        IComplaintRepository ComplaintRepository { get; }

        IWalletRepository Wallets {  get; } 
        IPaymentRepository Payments {  get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        ICraftsmanRepository Craftsmen { get; }
        IClientRepository Clients {  get; }

    }
}
