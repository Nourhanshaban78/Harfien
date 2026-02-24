using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Order
{
    public class CreateOrderDto
    {
        public int CraftsmanId { get; set; }
        public int ServiceId { get; set; }
    
        public string Description { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
    }
}
