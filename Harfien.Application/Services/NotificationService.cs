using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Harfien.Domain.Interface_Repository.Repositories;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(INotificationRepository notificationRepository, 
                               IHubContext<NotificationHub> hubContext)
        {
            _notificationRepository = notificationRepository;
            this._hubContext = hubContext;
        }

        public async Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId)
        {
            return await _notificationRepository.GetByUserIdAsync(userId);
        }

        public async Task MarkAsReadAsync(int notificationId)
        {
            var notification = await _notificationRepository.GetByIdAsync(notificationId);

            if (notification == null)
                throw new Exception("Notification not found");

            notification.IsRead = true;

            await _notificationRepository.SaveChangesAsync();
        }


        public async Task CreateNotificationAsync(string userId, string title, string message)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message
            };

       
            await _notificationRepository.AddAsync(notification);
            await _notificationRepository.SaveChangesAsync();

          
            await _hubContext.Clients.User(userId)
                .SendAsync("ReceiveNotification", new
                {
                    notification.Id,
                    notification.Title,
                    notification.Message,
                    notification.CreatedAt
                });
        }
       
    
        public async Task SendToMultipleUsersAsync(List<string> userIds, string title, string message)
        {
            foreach (var userId in userIds)
            {
                await CreateNotificationAsync(userId, title, message);
            }
        }
    }
}
