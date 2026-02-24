using Harfien.Application.DTO.Chat;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class ChatNotifier :IChatNotifier
    {
        private readonly IHubContext<ChatHub> _hub;

        public ChatNotifier(IHubContext<ChatHub> hub)
        {
            _hub = hub;
        }

        public async Task NotifyAsync(string receiverId, MessageDto message)
        {
            await _hub.Clients.User(receiverId)
                .SendAsync("ReceiveMessage", message);
        }
    }
}
