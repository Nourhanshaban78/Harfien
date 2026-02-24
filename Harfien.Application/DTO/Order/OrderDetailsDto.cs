using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Order
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public string ServiceName { get; set; } = null!;
        public string CraftsmanName { get; set; } = null!;
        public decimal Amount { get; set; }
        public OrderStatus Status { get; set; }
    }
}
