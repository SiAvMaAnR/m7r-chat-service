﻿using MessengerX.Application.Services.AdminService.Models;
using MessengerX.Application.Services.Common;

namespace MessengerX.Application.Services.AdminService;

public interface IAdminService : IBaseService
{
    Task<AdminServiceUsersResponse> GetUsersAsync(AdminServiceUsersRequest request);
    Task<AdminServiceUserResponse> GetUserAsync(AdminServiceUserRequest request);
    Task<AdminServiceProfileResponse> GetProfileAsync();
}
