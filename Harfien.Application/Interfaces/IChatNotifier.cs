using Harfien.Application.DTO.Chat;
using Harfien.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harfien.Application.Interfaces
{
    public interface IChatNotifier
    {
        Task NotifyAsync(string receiverId, MessageDto message);
    }
}
