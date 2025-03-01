using MyBaseProject.Application.DTOs.Chat;
using MyBaseProject.Application.Interfaces;
using MyBaseProject.Application.Interfaces.ExternalServices;
using MyBaseProject.Application.Interfaces.Repositories;
using MyBaseProject.Application.Mappers;

namespace MyBaseProject.Application.Services
{
    public class ChatService : IChatService
    {
        private readonly IPusherService _pusherService;
        private readonly IChatRepository _chatRepository;
        private readonly IAccountRepository _accountRepository;

        public ChatService(IPusherService pusherService, IChatRepository chatRepository, IAccountRepository accountRepository)
        {
            _pusherService = pusherService;
            _chatRepository = chatRepository;
            _accountRepository = accountRepository;
        }

        public async Task<ChatMessageResponseDto> SendMessageAsync(ChatMessageRequestDto request)
        {
            var chat = await _chatRepository.GetOrCreateChatAsync(request.SenderId, request.ReceiverId);

            var message = request.ToChatMessage();
            await _chatRepository.AddMessageToChatAsync(chat.Id, message);

            string receiverChannel = $"private-user-{request.ReceiverId}";
            await _pusherService.SendMessageAsync(receiverChannel, "new-message", message);

            return message.ToChatMessageResponseDto(request.ReceiverId);
        }

        public async Task<List<ChatResponseDto>> GetUserChatsAsync(string userId)
        {
            var chats = await _chatRepository.GetUserChatsAsync(userId);
            var chatDtos = new List<ChatResponseDto>();

            foreach (var chat in chats)
            {
                chatDtos.Add(await chat.ToChatResponseDto(_accountRepository, userId));
            }

            return chatDtos;
        }
        public async Task<ChatDetailResponseDto> GetChatByIdAsync(string chatId, string userId)
        {
            var chat = await _chatRepository.GetChatByIdAsync(chatId);
            if (chat == null) return null;

            return await chat.ToChatDetailResponseDto(_accountRepository, userId);
        }
    }
}
