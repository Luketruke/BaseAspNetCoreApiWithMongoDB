using Microsoft.AspNetCore.Mvc;
using MyBaseProject.Application.DTOs.Chat;
using MyBaseProject.Application.Interfaces;

namespace MyBaseProject.Api.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        /// <summary>
        /// Send a message to the chat if it already exists, or create a new chat before sending.
        /// </summary>
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] ChatMessageRequestDto request)
        {
            var response = await _chatService.SendMessageAsync(request);
            return Ok(response);
        }

        /// <summary>
        /// Gets all the user's chats with the last message sent.
        /// </summary>
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserChats(string userId)
        {
            var chats = await _chatService.GetUserChatsAsync(userId);
            return Ok(chats);
        }

        /// <summary>
        /// Get a chat through your Id.
        /// </summary>
        [HttpGet("{chatId}")]
        public async Task<IActionResult> GetChatById(string chatId, string userId)
        {
            var chat = await _chatService.GetChatByIdAsync(chatId, userId);
            if (chat == null) return NotFound();
            return Ok(chat);
        }
    }
}
