using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harfien.Domain.Entities;

namespace Harfien.Application.DTO.Service
{
    public class ServiceReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public int CraftsmanId { get; set; }
        
        public int ServiceCategoryId { get; set; }
        public string CraftsmanName { get; set; }
        public string ServiceCategoryName { get; set; }
        public string CraftsmanCity { get; set; }



    }
}
