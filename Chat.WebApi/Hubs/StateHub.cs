﻿using Chat.Application.Services.AccountService;
using Chat.Application.Services.AccountService.Models;
using Chat.Domain.Shared.Constants.Common;
using Chat.WebApi.Hubs.Common;
using Microsoft.AspNetCore.Authorization;

namespace Chat.WebApi.Hubs;

public class StateHub(IAccountService accountService) : BaseHub, IHub
{
    private readonly IAccountService _accountService = accountService;

    [Authorize]
    public override async Task OnConnectedAsync()
    {
        await _accountService.UpdateStatusAsync(
            new AccountServiceUpdateStatusRequest(AccountStatus.Online)
        );

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await _accountService.UpdateStatusAsync(
            new AccountServiceUpdateStatusRequest(AccountStatus.Offline)
        );

        await base.OnDisconnectedAsync(exception);
    }
}
