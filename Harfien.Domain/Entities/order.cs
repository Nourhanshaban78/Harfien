using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class Order : BaseEntity
    {
        [ForeignKey("ClientId")]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;
        [ForeignKey("CraftsmanId")]
        public int CraftsmanId { get; set; }
        public Craftsman Craftsman { get; set; } = null!;

        public int ServiceId { get; set; }
        public Service Service { get; set; } = null!;

        public string Description { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }

        public OrderStatus Status { get; set; }
        public decimal Amount { get; set; }

        public Payment Payment { get; set; } = null!;
    }
}
