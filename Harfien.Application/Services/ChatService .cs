using Harfien.Application.DTO.Chat;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository.Repositories;
using Harfien.Domain.Shared;

namespace Harfien.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IMessageRepositry _repository;
        private readonly IChatNotifier _notifier;

        public ChatService(IMessageRepositry repository,
                              IChatNotifier notifier)
        {
            _repository = repository;
            _notifier = notifier;
        }

        public async Task SendMessageAsync(string senderId, SendMessageRequest dto)
        {
            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId,
                Content = dto.Content
            };

            await _repository.AddAsync(message);
            await _repository.SaveChangesAsync();

            var messageDto = new MessageDto
            {
                SenderId = senderId,
                Content = dto.Content,
                SentAt = message.SentAt
            };

            await _notifier.NotifyAsync(dto.ReceiverId, messageDto);
        }

        public async Task<List<MessageDto>> GetConversationAsync(string user1, string user2)
        {
            var messages = await _repository.GetConversationAsync(user1, user2);

            return messages.Select(m => new MessageDto
            {
                SenderId = m.SenderId,
                Content = m.Content,
                SentAt = m.SentAt
            }).ToList();
        }


        public async Task<List<ChatListDto>> GetChatListAsync(string currentUserId)
        {
            return await _repository.GetChatListAsync(currentUserId);
        }

        public async Task MarkAsReadAsync(string senderId, string receiverId)
        {
            await _repository.MarkAsReadAsync(senderId, receiverId);
        }

    }
}