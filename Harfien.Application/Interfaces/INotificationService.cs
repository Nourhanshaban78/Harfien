using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface INotificationService
    {
        Task<IEnumerable<Notification>> GetUserNotificationsAsync(string userId);
        Task MarkAsReadAsync(int notificationId);
      

        Task CreateNotificationAsync(string userId, string title, string message);
        Task SendToMultipleUsersAsync(List<string> userIds, string title, string message);
    }
}
