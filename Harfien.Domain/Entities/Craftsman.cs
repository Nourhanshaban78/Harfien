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
    public class Craftsman:BaseEntity
    {
      

        public int NationalId { get; set; }
         public string Bio { get; set; }=string.Empty;

        public bool IsVerified { get; set; } = false;
        public double Rating { get; set; } = 0;

        [ForeignKey(nameof(UserId))]
        public string UserId { get; set; }    
        public ApplicationUser User { get; set; }
       
        public ICollection< Service> CraftsmanServices { get; set; }
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<CraftsmanAvailability> Availabilities { get; set; } = new List<CraftsmanAvailability>();
        public int YearsOfExperience { get; set; }
        public bool IsApproved { get; set; } = false;

    }

}
