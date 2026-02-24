using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Payment
{
    public class ClientPaymentDto
    {
        public int PaymentId { get; set; }
        public decimal Amount { get; set; }
        public string Status { get; set; } = null!;
        public string TransactionRef { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        // Order Info
        public int OrderId { get; set; }
        public string ServiceName { get; set; } = null!;
        public string CraftsmanName { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public decimal OrderAmount { get; set; }
    }
}
