using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{

    public class SubscriptionPlanDetails: BaseEntity
    {
        

        public string FeatureName { get; set; }
        public string FeatureDescription { get; set; }
        public int SubscriptionPlanId { get; set; }
        public SubscriptionPlan SubscriptionPlan { get; set; }

    }
}
