using Harfien.Application.DTO.Chat;
using Harfien.Application.Interfaces;
using Harfien.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
﻿using Harfien.Application.DTO;
using Harfien.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Harfien.Presentation.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> Send(SendMessageRequest dto)
        {
            var senderId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (senderId == null)
                return Unauthorized();

            await _chatService.SendMessageAsync(senderId, dto);

            return Ok();
        }

        [HttpGet("conversation/{receiverId}")]
        public async Task<IActionResult> GetConversation(string receiverId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (currentUserId == null)
                return Unauthorized();

            var messages = await _chatService
                .GetConversationAsync(currentUserId, receiverId);

            return Ok(messages);
        }

        [HttpGet("chat-list")]
        public async Task<IActionResult> GetChatList()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var chats = await _chatService.GetChatListAsync(currentUserId);

            return Ok(chats);
        }

        [HttpPost("mark-read/{senderId}")]
        public async Task<IActionResult> MarkAsRead(string senderId)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            await _chatService.MarkAsReadAsync(senderId, currentUserId);

            return Ok();
        }


    }
}
