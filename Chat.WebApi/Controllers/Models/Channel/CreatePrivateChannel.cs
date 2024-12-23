﻿namespace Chat.WebApi.Controllers.Models.Channel;

public class ChannelControllerCreatePrivateChannelRequest
{
    public required string Name { get; set; }
    public IEnumerable<int> Members { get; set; } = [];
    public int? AIProfileId { get; set; }
}
