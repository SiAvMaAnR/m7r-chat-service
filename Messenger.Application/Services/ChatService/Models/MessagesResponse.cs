﻿using Messenger.Domain.Shared.Models;

namespace Messenger.Application.Services.ChatService.Models;

public class ChatServiceAttachmentResponse
{
    public required int Id { get; set; }
    public required string Content { get; set; }
    public required string Type { get; set; }
}

public class ChatServiceMessageResponseData
{
    public int Id { get; set; }
    public string? Text { get; set; }
    public DateTime? ModifiedAt { get; set; }
    public bool IsRead { get; set; }
    public bool IsDeleted { get; set; }
    public int AuthorId { get; set; }
    public string? AuthorLogin { get; set; }
    public int ChannelId { get; set; }
    public DateTime CreatedAt { get; set; }
    public IEnumerable<ChatServiceAttachmentResponse> Attachments { get; set; } = [];
}

public class ChatServiceMessagesResponse
{
    public MetaResponse? Meta { get; set; }
    public IEnumerable<ChatServiceMessageResponseData>? Messages { get; set; }
}
