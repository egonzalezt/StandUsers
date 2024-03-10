namespace StandUsers.Workers.MessageBroker;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using HealthChecks;
using Domain.User;
using Workers.Exceptions;
using Workers.Extensions;
using System.Text.Json;
using System.Text;
using Domain.Centralizer;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.EntityFrameworkCore.DbContext;
using System;
using Infrastructure.MessageBroker.Options;

public class GovCarpetaWorker : BaseRabbitMQWorker
{
    private readonly ILogger<GovCarpetaWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public GovCarpetaWorker(
        ILogger<GovCarpetaWorker> logger,
        IServiceProvider serviceProvider,
        ConnectionFactory rabbitConnection,
        IHealthCheckNotifier healthCheckNotifier,
        SystemStatusMonitor statusMonitor,
        IOptions<ConsumerConfiguration> queues
    ) : base(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.UserReplyQueue)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
    }

    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var userId = headers.GetHeaderValue("UserId");
        var operation = headers.GetUserEventType();
        _logger.LogInformation("Processing request for user {userId}", userId);
        using var scope = _serviceProvider.CreateScope();
        if(operation == UserOperations.CreateUser)
        {
            var createUser = JsonSerializer.Deserialize<GenericResponse<string>>(message) ?? throw new InvalidBodyException();
            var useCase = scope.ServiceProvider.GetServices<ICentralizerUserUseCase>().First(s => s.UseCase == operation);
            await useCase.ExecuteAsync(createUser, userId);
            var database = scope.ServiceProvider.GetRequiredService<StandUsersDbContext>();
            await database.SaveChangesAsync();
        }
        channel.BasicAck(eventArgs.DeliveryTag, false);
        return;
    }
}
