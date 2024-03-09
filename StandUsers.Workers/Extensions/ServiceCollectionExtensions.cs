namespace StandUsers.Workers.Extensions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using MessageBroker.Options;
using MessageBroker;

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
        services.Configure<ConsumerConfiguration>(configuration.GetSection("RabbitMQ:Queues"));
        services.AddHostedService<UsersWorker>();
    }
}