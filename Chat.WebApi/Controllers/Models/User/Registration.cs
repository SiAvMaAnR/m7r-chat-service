﻿using System.ComponentModel.DataAnnotations;
using Chat.Domain.Shared.Constants.Validation;

namespace Chat.WebApi.Controllers.Models.User;

public class UserControllerRegistrationRequest
{
    [MaxLength(MaxLength.Login)]
    public required string Login { get; set; }

    [EmailAddress, MaxLength(MaxLength.Email)]
    public required string Email { get; set; }

    [MaxLength(MaxLength.Password)]
    public required string Password { get; set; }
    public DateOnly? Birthday { get; set; }
}
