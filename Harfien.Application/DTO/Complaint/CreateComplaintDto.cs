using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Complaint
{

    public class CreateComplaintDto
    {
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string? EvidenceAttachmentUrl { get; set; }
    }

}

