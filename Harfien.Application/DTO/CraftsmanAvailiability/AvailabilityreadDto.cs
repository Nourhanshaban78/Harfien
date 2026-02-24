using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.CraftsmanAvailiability
{
    public class AvailabilityreadDto
    {

       public int CraftsmanId { get; internal set; }
        public int Day { get; set; }
        public TimeSpan StartTime  { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
