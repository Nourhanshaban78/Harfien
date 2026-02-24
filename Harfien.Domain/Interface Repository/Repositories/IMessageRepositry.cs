using Harfien.Domain.Entities;
using Harfien.Domain.Shared;

namespace Harfien.Domain.Interface_Repository.Repositories
{
    public interface IMessageRepositry
    {
        Task AddAsync(ChatMessage message);
        Task<List<ChatMessage>> GetConversationAsync(string user1, string user2);
        Task SaveChangesAsync();

        Task<List<ChatListDto>> GetChatListAsync(string currentUserId);
        Task MarkAsReadAsync(string senderId, string receiverId);

    }
}
