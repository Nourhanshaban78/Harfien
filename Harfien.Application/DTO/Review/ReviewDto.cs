using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Review
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateOnly CreatedAt { get; set; }
    }
}
