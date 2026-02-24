using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Shared
{
    public class ServiceQueryDto
    {
        public int? CategoryId { get; set; }
        public int? CraftnamId { get; set; }

        public string? Area { get; set; }
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
