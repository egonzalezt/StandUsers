namespace StandUsers.Workers.MessageBroker;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HealthChecks;
using Workers.Exceptions;
using Workers.MessageBroker.Options;
using System.Text.Json;
using System.Text;
using Workers.Extensions;
using Domain.User;
using Domain.User.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

public class UsersWorker : BaseRabbitMQWorker
{
    private readonly ILogger<UsersWorker> _logger;
    private readonly IServiceProvider _serviceProvider;

    public UsersWorker(
        ILogger<UsersWorker> logger,
        ConnectionFactory rabbitConnection,
        IHealthCheckNotifier healthCheckNotifier,
        SystemStatusMonitor statusMonitor,
        IOptions<ConsumerConfiguration> queues
    ) : base(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.CreateUserQueue)
    {
        _logger = logger;
    }

    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = _serviceProvider.CreateScope();
        switch (operation)
        {
            case UserOperations.CreateUser:
                var userDto = JsonSerializer.Deserialize<UserDto>(message) ?? throw new InvalidBodyException();
                _logger.LogInformation("Processing request for user {userId}", userDto.IdentificationNumber);
                var useCaseSelector = scope.ServiceProvider.GetRequiredService<ICreateUserUseCase>();
                await useCaseSelector.ExecuteAsync(userDto);
                break;
            default:
                _logger.LogWarning("Not supported Operation: {0}", operation);
                throw new InvalidEventTypeException();
        }
    }
}
