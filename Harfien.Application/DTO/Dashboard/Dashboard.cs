using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Dashboard
{
    public class Dashboard
    {
       
            public int TotalCraftmen { get; set; }
            public int TotalUsers { get; set; }
            public int TotalServices { get; set; }
            public int CompletedOrdersCount { get; set; }
            public decimal TotalProfit { get; set; }
        
    }
}
