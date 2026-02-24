using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.CraftsmanAvailiability
{
    public class CreateAvailabilityDto
    {
        [Range(0, 6, ErrorMessage = "Day must be between 0 (Sunday) and 6 (Saturday).")]
        public int Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAvailable { get; set; }
    }
}
