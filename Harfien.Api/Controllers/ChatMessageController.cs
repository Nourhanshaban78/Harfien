using Harfien.Application.DTO;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatMessageController : ControllerBase
    {
        private readonly IChatMessageService _chatMessageService;

        public ChatMessageController(IChatMessageService chatMessageService)
        {
            _chatMessageService = chatMessageService;
        }

        // POST: api/chatmessage
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Send([FromBody] AddChatMessageDto dto)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (senderId == null)
                return Unauthorized();

            try
            {
                await _chatMessageService.SendMessageAsync(dto, senderId);
                return Ok("Message Sent Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/chatmessage/chat/1
        [HttpGet("chat/{chatId}")]
        public async Task<IActionResult> GetChatMessages(int chatId)
        {
            var messages = await _chatMessageService.GetMessagesByChatIdAsync(chatId);
            return Ok(messages);
        }
    }
}



