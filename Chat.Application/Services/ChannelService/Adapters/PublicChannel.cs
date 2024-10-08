﻿using Chat.Application.Services.ChannelService.Models;
using Chat.Domain.Entities.Accounts;
using Chat.Domain.Entities.Channels;
using Chat.Domain.Shared.Constants.Common;
using Chat.Persistence.Extensions;

namespace Chat.Application.Services.ChatService.Adapters;

public class ChannelServicePublicChannelAdapter : ChannelServicePublicChannelResponseData
{
    private readonly string? _imagePath;

    public ChannelServicePublicChannelAdapter(Channel channel, int authorId)
    {
        Id = channel.Id;
        Type = channel.Type;
        LastActivity = channel.LastActivity;

        if (Type == ChannelType.Direct)
        {
            Account? chatPartner = channel
                .Accounts
                .FirstOrDefault(account => account.Id != authorId);

            if (chatPartner != null)
            {
                _imagePath = chatPartner.Image;
                Name = chatPartner.Login;
            }
        }
        else
        {
            _imagePath = channel.Image;
            Name = channel.Name;
        }
    }

    public async Task LoadImageAsync()
    {
        Image = await FileManager.ReadToBytesAsync(_imagePath);
    }
}
