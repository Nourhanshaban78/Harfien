using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Harfien.Domain.Entities
{
    public class WalletTransaction: BaseEntity
    {
       
        public int WalletId { get; set; }
        public int? OrderId { get; set; }

        public TransactionType Type { get; set; } // Credit, Debit
        public decimal Amount { get; set; }
        public string TransactionReason { get; set; }
        public string Reference { get; set; }

        public Enums.TransactionStatus Status { get; set; }
       

        public Wallet Wallet { get; set; }
        public Order Order { get; set; }
        

    }
}
