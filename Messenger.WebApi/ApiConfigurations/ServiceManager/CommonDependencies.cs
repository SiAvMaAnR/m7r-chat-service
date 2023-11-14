using MessengerX.Infrastructure.AuthOptions;
using MessengerX.Persistence.DBContext;
using MessengerX.WebApi.ApiConfigurations.Common;
using MessengerX.WebApi.ApiConfigurations.Other;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace MessengerX.WebApi.ApiConfigurations.ServiceManager;

public static partial class ServiceManagerExtension
{
    public static IServiceCollection AddCommonDependencies(
        this IServiceCollection serviceCollection,
        IConfiguration config
    )
    {
        string connection = config.GetConnectionString("DefaultConnection") ?? "";

        serviceCollection.AddOptions();
        serviceCollection.AddDbContext<EFContext>(options => options.UseSqlServer(connection));
        serviceCollection.AddControllers();
        serviceCollection.AddEndpointsApiExplorer();
        serviceCollection.AddHttpContextAccessor();
        serviceCollection.AddLogging();
        serviceCollection.AddCors(options => options.CorsConfig());
        serviceCollection.AddAuthorization(options => options.PolicyConfig());
        serviceCollection.AddSwaggerGen(options => options.Config());
        serviceCollection
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => options.Config(config));
        serviceCollection.AddDataProtection();
        serviceCollection.AddSignalR();

        return serviceCollection;
    }
}