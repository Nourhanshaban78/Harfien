using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class CraftsmanAvailability  :BaseEntity
    {


        public int CraftsmanId { get; set; }
        public Craftsman Craftsman { get; set; } = null!;

        public int Day { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }

        public bool IsAvailable { get; set; }
    }

}
