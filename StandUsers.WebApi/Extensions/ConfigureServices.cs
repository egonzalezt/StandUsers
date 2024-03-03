namespace StandUsers.WebApi.Extensions;

using Application.Services.Configuration;
using Infrastructure.Services.Configuration;

public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddInfrastructure(configuration);
        services.AddApplication();
        services.ConfigureSwagger();
    }
}
