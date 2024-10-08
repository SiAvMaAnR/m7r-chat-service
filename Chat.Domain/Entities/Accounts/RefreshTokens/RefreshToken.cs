﻿using System.ComponentModel.DataAnnotations.Schema;
using Chat.Domain.Entities.Accounts;

namespace Chat.Domain.Entities.RefreshTokens;

[Table("RefreshTokens")]
public partial class RefreshToken : BaseEntity
{
    public RefreshToken(string token, DateTime expiryTime, int accountId)
    {
        Token = token;
        ExpiryTime = expiryTime;
        AccountId = accountId;
    }

    public string Token { get; private set; }
    public DateTime ExpiryTime { get; private set; }
    public int AccountId { get; private set; }
    public Account? Account { get; private set; }
}
