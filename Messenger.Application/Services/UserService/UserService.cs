﻿using System.Text.Json;
using MessengerX.Application.Services.Common;
using MessengerX.Application.Services.UserService.Models;
using MessengerX.Domain.Entities.Users;
using MessengerX.Domain.Exceptions.BusinessExceptions;
using MessengerX.Domain.Interfaces.UnitOfWork;
using MessengerX.Domain.Shared.Models;
using MessengerX.Infrastructure.AppSettings;
using MessengerX.Infrastructure.AuthOptions;
using MessengerX.Infrastructure.NotificationTemplates;
using MessengerX.Notifications;
using MessengerX.Notifications.Common;
using MessengerX.Notifications.Email.Models;
using MessengerX.Persistence.Extensions;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;

namespace MessengerX.Application.Services.UserService;

public class UserService : BaseService, IUserService
{
    private readonly IDataProtectionProvider _protection;
    private readonly INotificationClient _emailClient;

    public UserService(
        IUnitOfWork unitOfWork,
        IHttpContextAccessor context,
        IAppSettings appSettings,
        IDataProtectionProvider protection,
        EmailClient emailClient
    )
        : base(unitOfWork, context, appSettings)
    {
        _protection = protection;
        _emailClient = emailClient;
    }

    public async Task<UserServiceRegistrationResponse> RegistrationAsync(
        UserServiceRegistrationRequest request
    )
    {
        if (await _unitOfWork.User.AnyAsync(user => user.Email == request.Email))
            throw new AlreadyExistsException("Account already exists");

        string baseUrl = _appSettings.Client.BaseUrl;

        string path = _appSettings.RoutePath.Registration;

        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);

        string confirmationJson = JsonSerializer.Serialize(
            new Confirmation()
            {
                Login = request.Login,
                Email = request.Email,
                Password = request.Password,
                Birthday = request.Birthday
            }
        );

        string confirmation = protector.Protect(confirmationJson);

        string confirmationLink = $"{baseUrl}/{path}?code={confirmation}";

        string smtpEmail = _appSettings.Smtp.Email;

        EmailTemplate template = NotificationTemplate.Registration(confirmationLink);

        var message = new Message()
        {
            From = new Address(baseUrl, smtpEmail),
            To = new Address(request.Login, request.Email),
            Subject = template.Subject,
            Content = template.Content
        };

        await _emailClient.SendAsync(message);

        return new UserServiceRegistrationResponse() { IsSuccess = true };
    }

    public async Task<UserServiceConfirmationResponse> ConfirmationAsync(
        UserServiceConfirmationRequest request
    )
    {
        string secretKey = _appSettings.Common.SecretKey;

        IDataProtector protector = _protection.CreateProtector(secretKey);
        string confirmationJson = protector.Unprotect(request.Confirmation);

        Confirmation confirmation =
            JsonSerializer.Deserialize<Confirmation>(confirmationJson)
            ?? throw new InvalidConfirmationException("Invalid confirmation");

        if (await _unitOfWork.Account.AnyAsync(account => account.Email == confirmation.Email))
            throw new AlreadyExistsException("Account already exists");

        Password password = PasswordOptions.CreatePasswordHash(confirmation.Password);

        var user = new User()
        {
            Login = confirmation.Login,
            Email = confirmation.Email,
            PasswordHash = password.Hash,
            PasswordSalt = password.Salt,
            Birthday = confirmation.Birthday,
        };

        await _unitOfWork.User.AddAsync(user);

        await _unitOfWork.SaveChangesAsync();

        return new UserServiceConfirmationResponse()
        {
            Email = confirmation.Email,
            Password = confirmation.Password
        };
    }

    public async Task<UserServiceProfileResponse> GetProfileAsync()
    {
        User user =
            await _unitOfWork.User.GetAsync((user) => user.Id == _userIdentity.Id)
            ?? throw new NotExistsException("User not found");

        return new UserServiceProfileResponse()
        {
            Login = user.Login,
            Email = user.Email,
            Role = user.Role,
            Birthday = user.Birthday
        };
    }

    public async Task<UserServiceImageResponse> GetImageAsync()
    {
        User user =
            await _unitOfWork.User.GetAsync((user) => user.Id == _userIdentity.Id)
            ?? throw new NotExistsException("User not found");

        byte[]? image = await user.Image.ReadToBytesAsync();

        return new UserServiceImageResponse() { Image = image };
    }

    public async Task<UserServiceUploadImageResponse> UploadImageAsync(
        UserServiceUploadImageRequest request
    )
    {
        User user =
            await _unitOfWork.User.GetAsync((user) => user.Id == _userIdentity.Id)
            ?? throw new NotExistsException("User not found");

        string imagePath = _appSettings.FilePath.Image;

        using (var stream = new MemoryStream())
        {
            request.File.CopyTo(stream);
            await stream.ToArray().WriteToFileAsync(imagePath, user.Email);
        }

        return new UserServiceUploadImageResponse() { IsSuccess = true };
    }
}
