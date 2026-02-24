using Harfien.DataAccess;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository.Repositories;
using Microsoft.EntityFrameworkCore;
using Harfien.Domain.Shared;


namespace Harfien.Infrastructure.Repositories
{
    public class MessageRepositry : IMessageRepositry
    {
        private readonly HarfienDbContext _context;

        public MessageRepositry(HarfienDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ChatMessage message)
        {
            await _context.ChatMessages.AddAsync(message);
        }

        public async Task<List<ChatMessage>> GetConversationAsync(string user1, string user2)
        {
            return await _context.ChatMessages
                .Where(m =>
                    (m.SenderId == user1 && m.ReceiverId == user2) ||
                    (m.SenderId == user2 && m.ReceiverId == user1))
                .OrderBy(m => m.SentAt)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


        public async Task<List<ChatListDto>> GetChatListAsync(string currentUserId)
        {
            var messages = await _context.ChatMessages
                .Where(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
                .ToListAsync();

            var chatList = messages
                .GroupBy(m => m.SenderId == currentUserId ? m.ReceiverId : m.SenderId)
                .Select(g => new ChatListDto
                {
                    UserId = g.Key,
                    LastMessage = g.OrderByDescending(x => x.SentAt).First().Content,
                    LastMessageTime = g.Max(x => x.SentAt),
                    UnreadCount = g.Count(x =>
                        x.ReceiverId == currentUserId && !x.IsRead)
                })
                .OrderByDescending(x => x.LastMessageTime)
                .ToList();

            return chatList;
        }

        public async Task MarkAsReadAsync(string senderId, string receiverId)
        {
            var unreadMessages = await _context.ChatMessages
                .Where(m =>
                    m.SenderId == senderId &&
                    m.ReceiverId == receiverId &&
                    !m.IsRead)
                .ToListAsync();

            foreach (var msg in unreadMessages)
                msg.IsRead = true;

            await _context.SaveChangesAsync();
        }

    }
}
