using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{

    public class SubscriptionPlan: BaseEntity
    {

        public string Name { get; set; }
       
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }

        public bool IsActive { get; set; } = true;

        public ICollection<SubscriptionPlanDetails> SubscriptionPlanDetails { get; set; }

    }
}
