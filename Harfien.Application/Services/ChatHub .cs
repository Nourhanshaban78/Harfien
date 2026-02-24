using Harfien.Application.DTO.Chat;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class ChatHub :Hub
    {
        private readonly IChatService _chatService;

        public ChatHub(IChatService chatService)
        {
            _chatService = chatService;
        }

        public async Task SendMessage(string receiverId, string message)
        {
            var senderId = Context.UserIdentifier;

            await _chatService.SendMessageAsync(senderId,
                new SendMessageRequest
                {
                    ReceiverId = receiverId,
                    Content = message
                });

            await Clients.User(receiverId)
                .SendAsync("ReceiveMessage", senderId, message);

            await Clients.Caller
                .SendAsync("ReceiveMessage", senderId, message);
        }
    }
}
