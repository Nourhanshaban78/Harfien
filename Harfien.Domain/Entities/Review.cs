using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class Review : BaseEntity
    {
        public int Rating { get; set; } // Use int for easier math (Average Rating) // Range: 1 - 5
        public string? Comment { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        // Explicit IDs for faster querying of a Craftsman's profile
        public int ClientId { get; set; } // The Reviewer
        public int CraftsmanId { get; set; } // The Target
    }
}
