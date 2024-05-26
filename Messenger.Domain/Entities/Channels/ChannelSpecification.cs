﻿using MessengerX.Domain.Shared.Constants.Common;
using MessengerX.Domain.Specification;

namespace MessengerX.Domain.Entities.Channels;

public class PublicChannelsSpec : Specification<Channel>
{
    public PublicChannelsSpec(string? searchField)
        : base(
            (channel) =>
                channel.Type == ChannelType.Public
                && (
                    searchField == null
                    || channel.Name != null && channel.Name.Contains(searchField)
                )
        ) { }
}

public class AccountChannelsSpec : Specification<Channel>
{
    public AccountChannelsSpec(int? accountId, string? searchField, string? channelType)
        : base(
            (channel) =>
                channel.Accounts.Any(account => account.Id == accountId)
                && (channelType == null || channel.Type == channelType)
                && (
                    searchField == null
                    || channel.Name != null && channel.Name.Contains(searchField)
                    || channel.Type == ChannelType.Direct
                        && channel
                            .Accounts
                            .Any(
                                account =>
                                    account.Id != accountId && account.Login.Contains(searchField)
                            )
                )
        )
    {
        AddInclude(channel => channel.Accounts);
        AddInclude(channel => channel.Messages);
        AddInclude("Messages.Author");
    }
}

public class AccountChannelSpec : Specification<Channel>
{
    public AccountChannelSpec(int? accountId, int channelId)
        : base(
            (channel) =>
                channel.Accounts.Any(account => account.Id == accountId) && channel.Id == channelId
        )
    {
        AddInclude(channel => channel.Accounts);
    }
}

public class ChannelByIdSpec : Specification<Channel>
{
    public ChannelByIdSpec(int? id)
        : base((channel) => channel.Id == id) { }
}