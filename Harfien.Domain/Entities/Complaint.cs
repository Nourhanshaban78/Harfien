using Harfien.Domain.Entities;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Entities
{
    public class Complaint : BaseEntity
    {
        public int ReporterId { get; set; } // Client or Craftsman who is reporting the issue
        public string Description { get; set; }
        public string? EvidenceAttachmentUrl { get; set; } // link to image/video evidence
        public ComplaintStatus Status { get; set; } = ComplaintStatus.Pending;
        // Admin resolution details
        public string? AdminResolutionNotes { get; set; }
        public DateTime? ResolvedAt { get; set; }
        // Navigation Properties
        // will give error for now as order is not defined yet
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
