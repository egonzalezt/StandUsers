namespace StandUsers.Workers.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using MessageBroker;
using Infrastructure.MessageBroker.Options;

public static class ServiceCollectionExtensions
{
    public static void AddWorkers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ConnectionFactory>(sp =>
        {
            var factory = new ConnectionFactory();
            configuration.GetSection("RabbitMQ:Connection").Bind(factory);
            return factory;
        });

        services.Configure<ConsumerConfiguration>(options =>
            configuration.GetSection("RabbitMQ:Queues:Consumer").Bind(options)
        );
        services.Configure<PublisherConfiguration>(options =>
            configuration.GetSection("RabbitMQ:Queues:Publisher").Bind(options)
        );

        services.AddHostedService<UsersWorker>();
        services.AddHostedService<GovCarpetaWorker>();
    }
}