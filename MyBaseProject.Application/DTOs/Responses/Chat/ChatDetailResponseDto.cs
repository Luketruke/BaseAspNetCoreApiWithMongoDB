using System;
using System.Collections.Generic;
using MyBaseProject.Application.DTOs.Responses;

namespace MyBaseProject.Application.DTOs.Chat
{
    public class ChatDetailResponseDto
    {
        public string ChatId { get; set; }
        public AccountResponseDto User { get; set; }
        public List<ChatMessageResponseDto> Messages { get; set; } = new();
    }
}
