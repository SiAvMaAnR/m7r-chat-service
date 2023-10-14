using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;

namespace MessengerX.WebApi.Config.CommonConfig;

public static class PolicyConfigExtension
{
    public static void PolicyConfig(this AuthorizationOptions authorizationOptions)
    {
        authorizationOptions.AddPolicy("OnlyUser", policy => policy.RequireRole("User"));
        authorizationOptions.AddPolicy("OnlyAdmin", policy => policy.RequireRole("Admin"));
        authorizationOptions.AddPolicy(
            "AccessToAll",
            policy => policy.RequireRole("User").RequireRole("Admin")
        );
    }

    public static void CorsConfig(this CorsOptions corsOptions)
    {
        corsOptions.AddPolicy(
            "CorsPolicy",
            policy =>
                policy
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials()
                    .WithOrigins("http://localhost:3000", "https://localhost:3000")
        );
        // .AllowAnyOrigin());
    }
}
