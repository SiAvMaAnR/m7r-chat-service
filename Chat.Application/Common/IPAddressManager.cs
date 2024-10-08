﻿using Microsoft.AspNetCore.Http;

namespace Chat.Application.Common;

public class IPAddressManager
{
    private readonly HttpContext? _context;

    public IPAddressManager(IHttpContextAccessor httpContextAccessor)
    {
        _context = httpContextAccessor.HttpContext;
    }

    public string? ClientIP => _context?.Connection.RemoteIpAddress?.ToString();
}
