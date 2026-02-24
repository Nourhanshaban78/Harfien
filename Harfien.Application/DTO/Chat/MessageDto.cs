using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.DTO.Chat
{
    public class MessageDto
    {
        public string SenderId { get; set; }
        public string Content { get; set; }
        public DateTime SentAt{get; set;}
    }
}
