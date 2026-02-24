using Harfien.Application.DTO.Order;
using Harfien.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Complaint
{
    public class ComplaintDetailsDto
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string? EvidenceAttachmentUrl { get; set; }
        public ComplaintStatus Status { get; set; }
        public string? AdminResolutionNotes { get; set; }

        public DateTime? ResolvedAt { get; set; }

        public OrderInfoDto Order { get; set; }
    }


}
