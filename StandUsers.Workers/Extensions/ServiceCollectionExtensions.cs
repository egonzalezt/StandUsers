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
        services.Configure<ConsumerConfiguration>(configuration.GetSection("RabbitMQ:Queues:Consumer"));
        services.Configure<PublisherConfiguration>(configuration.GetSection("RabbitMQ:Queues:Publisher"));
        services.AddHostedService<UsersWorker>();
        services.AddHostedService<GovCarpetaWorker>();
    }
}