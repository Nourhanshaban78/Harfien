using Harfien.Application.DTO.Chat;
using Harfien.Domain.Shared;

namespace Harfien.Application.Interfaces
{
    public interface IChatService
    {
        Task SendMessageAsync(string senderId, SendMessageRequest dto);
        Task<List<MessageDto>> GetConversationAsync(string user1, string user2);

        Task<List<ChatListDto>> GetChatListAsync(string currentUserId);
        Task MarkAsReadAsync(string senderId, string receiverId);

    }
}
