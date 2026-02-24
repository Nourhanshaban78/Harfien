using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;

namespace Harfien.Domain.Entities
{
    public class Client:BaseEntity
    {
       


        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }



        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<SubscriptionPlanDetails> Subscriptions { get; set; }
    }
}
