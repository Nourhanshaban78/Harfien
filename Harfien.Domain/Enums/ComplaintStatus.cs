using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Domain.Enums
{
    public enum ComplaintStatus
    {
        Pending,        // Just submitted
        Investigating,  // Admin is looking into it
        Resolved,       // Issue fixed / Refund issued
        Dismissed       // Complaint was invalid
    }
}
