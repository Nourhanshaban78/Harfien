using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Complaint
{
    public class UpdateComplaintDto
    {
        public string Description { get; set; }
        public string? EvidenceAttachmentUrl { get; set; }
    }

}
