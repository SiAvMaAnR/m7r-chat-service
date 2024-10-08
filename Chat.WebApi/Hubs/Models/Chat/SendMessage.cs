﻿namespace Chat.WebApi.Hubs.Models.Chat;

public class ChatHubSendMessageRequest
{
    public required int ChannelId { get; set; }
    public required string Message { get; set; }
    public IEnumerable<string> Attachments { get; set; } = [];
}
