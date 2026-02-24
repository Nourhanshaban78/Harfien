using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Order
{
    public class OrderInfoDto
    {
        public int OrderId { get; set; }
        public string ClientName { get; set; }
        public string CraftsmanName { get; set; }
        public string ServiceName { get; set; }

        public DateTime ScheduledAt { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal Amount { get; set; }
    }

}
