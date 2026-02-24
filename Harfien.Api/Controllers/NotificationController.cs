using Harfien.Application.DTO.Notifications;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]

    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;


        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return BadRequest("No Have Notification");
            var notifications =
                await _notificationService.GetUserNotificationsAsync(userId);
           
            return Ok(notifications);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return NoContent();
        }


        [HttpPost("send")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequestDto request)
        {
            await _notificationService.CreateNotificationAsync(request.UserId, request.Title, request.Message);
            return Ok("Notification sent successfully");
        }

        [HttpPost("send-multiple")]
        public async Task<IActionResult> SendMultipleNotifications([FromBody] NotificationMultipleRequestDto request)
        {
            await _notificationService.SendToMultipleUsersAsync(request.UserIds, request.Title, request.Message);
            return Ok("Notifications sent successfully");
        }
    }
}
