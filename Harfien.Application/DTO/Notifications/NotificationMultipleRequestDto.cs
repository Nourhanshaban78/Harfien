using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Notifications
{
    public class NotificationMultipleRequestDto
    {
        public List<string> UserIds { get; set; } = new List<string>();
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
