using Harfien.Domain.Entities;
using Harfien.Domain.Enums;

namespace Harfien.Domain.Entities
{
    public class Payment :BaseEntity
    {

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;

        public decimal Amount { get; set; }
        public PaymentStatus Status { get; set; }

        public string TransactionRef { get; set; } = null!;
    }
}
