﻿namespace Chat.Domain.Shared.Settings;

public class RMQSettings : ISettings
{
    public static string Path => "RMQ";

    public string HostName { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Password { get; set; } = null!;
    public int Timeout { get; set; }
}
