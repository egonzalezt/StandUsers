namespace StandUsers.WebApi.Extensions;

using Application.Services.Configuration;
using Infrastructure.Services.Configuration;
using Workers.Extensions;
using HealthChecks.Extensions;
public static class ConfigureServices
{
    public static void AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecksServices(configuration);
        services.AddInfrastructure(configuration);
        services.AddWorkers(configuration);
        services.AddApplication();
        services.ConfigureSwagger();
    }
}
