namespace StandUsers.Workers.MessageBroker;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using HealthChecks;
using Workers.Exceptions;
using System.Text.Json;
using System.Text;
using Workers.Extensions;
using Domain.User;
using Domain.User.Dtos;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.EntityFrameworkCore.DbContext;
using Domain.Centralizer.Dtos;
using Domain.SharedKernel;
using Infrastructure.MessageBroker.Options;
using StandUsers.Infrastructure.MessageBroker;

public class UsersWorker(
    ILogger<UsersWorker> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues,
    IOptions<PublisherConfiguration> publisherQueue,
    IOptions<OperatorIdentification> providerIdentification
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.CreateUserQueue)
{
    PublisherConfiguration _publisherQueue = publisherQueue.Value;
    OperatorIdentification _providerIdentification = providerIdentification.Value;

    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if(operation is UserOperations.CreateUser)
        {
            var userDto = JsonSerializer.Deserialize<UserDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing request for user {userId}", userDto.IdentificationNumber);
            var useCaseSelector = scope.ServiceProvider.GetRequiredService<ICreateUserUseCase>();
            var user = await useCaseSelector.ExecuteAsync(userDto);
            var createUserDto = CreateUserDto.Build(user, _providerIdentification);
            var requestHeaders = new EventHeaders(operation.ToString(), user.Id);
            var properties = channel.CreateBasicProperties();
            properties.Headers = requestHeaders.GetAttributesAsDictionary();
            string jsonResult = JsonSerializer.Serialize(createUserDto);
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonResult);
            var requestQueue = _publisherQueue.UserRequestQueue;
            channel.BasicPublish("", requestQueue, properties, jsonBytes);
            var database = scope.ServiceProvider.GetRequiredService<StandUsersDbContext>();
            await database.SaveChangesAsync();
            channel.BasicAck(eventArgs.DeliveryTag, false);
        }
    }
}
