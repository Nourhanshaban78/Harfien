using Harfien.Application.DTO.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Payment
{
    public class WalletTransactionDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public int? OrderId { get; set; }
        public OrderDetailsDto OrderDetails { get; set; }
        public string Reference { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
