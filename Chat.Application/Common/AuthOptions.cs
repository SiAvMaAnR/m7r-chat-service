﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Chat.Domain.Shared.Constants.Common;
using Chat.Domain.Shared.Settings;
using Chat.Infrastructure.AppSettings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;

namespace Chat.Application.Common;

public static class AuthOptions
{
    public static void Config(this JwtBearerOptions options, IConfiguration configuration)
    {
        CommonSettings commonSettings = AppSettings.GetSection<CommonSettings>(configuration);
        AuthSettings authSettings = AppSettings.GetSection<AuthSettings>(configuration);

        string? issuer = authSettings.Issuer;
        string? audience = authSettings.Audience;

        byte[] hashSecretKey = SHA512.HashData(Encoding.UTF8.GetBytes(commonSettings.SecretKey));

        // options.RequireHttpsMetadata = true;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(hashSecretKey),
            LifetimeValidator = (
                DateTime? notBefore,
                DateTime? expires,
                SecurityToken securityToken,
                TokenValidationParameters validationParameters
            ) => (expires != null) && DateTime.UtcNow < expires
        };
        options.Events = new JwtBearerEvents()
        {
            OnMessageReceived = context =>
            {
                StringValues accessToken = context.Request.Query["access_token"];

                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
    }

    public static string CreateAccessToken(
        List<Claim> claims,
        Dictionary<string, string> tokenParams
    )
    {
        byte[] hashSecretKey = SHA512.HashData(
            Encoding.UTF8.GetBytes(tokenParams[TokenClaim.SecretKey])
        );
        var key = new SymmetricSecurityKey(hashSecretKey);

        DateTime expires = DateTime
            .Now
            .AddMinutes(double.Parse(tokenParams[TokenClaim.AccessTokenLifeTime]));

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            audience: tokenParams[TokenClaim.Audience],
            issuer: tokenParams[TokenClaim.Issuer],
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static string CreateRefreshToken()
    {
        byte[] randomNumber = new byte[256];
        RandomNumberGenerator.Create().GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
}
