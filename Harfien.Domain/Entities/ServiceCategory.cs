using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class ServiceCategory: BaseEntity

    {
        
        public string Type { get; set; }
        public string Description { get; set; }

        public ICollection<Service> Services { get; set; } = new List<Service>();
        public string Name { get; set; }
    }
}
        