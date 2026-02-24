using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Payment
{
    public class CreateWalletTransactionDto
    {
        public decimal Amount { get; set; }
        public TransactionType Type { get; set; } // Credit / Debit
        public string Reason { get; set; }
    }

}
