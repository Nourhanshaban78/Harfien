using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Dashboard
{
    public class CraftmanDto
    {
       
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }

            public double Rating { get; set; }
            public bool IsVerified { get; set; }
        
    }
}
