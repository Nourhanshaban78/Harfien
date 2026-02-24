using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Payment
{
    public class WalletDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
        public bool IsActive { get; set; }
        public List<WalletTransactionDto> Transactions { get; set; } = new();
    }
}
