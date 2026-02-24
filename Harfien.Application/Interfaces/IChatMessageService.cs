using Harfien.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface IChatMessageService
    {
        Task SendMessageAsync(AddChatMessageDto dto, string senderId);
        Task<object> GetMessagesByChatIdAsync(int chatId);
    }
}
