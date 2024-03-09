namespace StandUsers.HealthChecks.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

public static class ServiceCollectionExtensions
{
    public static void AddHealthChecksServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<SystemStatusMonitor>();
        services.AddSingleton<IHealthCheckNotifier, HealthCheckNotifier>();
        services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();
        var intervalMinutes = configuration.GetValue<int>("HealthChecks:IntervalMinutes");
        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Period = TimeSpan.FromMinutes(intervalMinutes);
        });
    }
}
