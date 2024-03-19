namespace StandUsers.Workers.MessageBroker;

using Frieren_Guard;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Domain.SharedKernel;
using Infrastructure.MessageBroker.Options;
using System.Threading.Tasks;
using RabbitMQ.Client.Events;
using Microsoft.Extensions.DependencyInjection;
using Workers.Extensions;
using System.Text;
using Domain.User.Dtos;
using Workers.Exceptions;
using System.Text.Json;
using Application.Interfaces;
using Infrastructure.EntityFrameworkCore.DbContext;
using Domain.Centralizer.Dtos;
using Domain.User;
using Infrastructure.MessageBroker;

public class TransferWorker(
    ILogger<UsersWorker> logger,
    IServiceProvider serviceProvider,
    ConnectionFactory rabbitConnection,
    IHealthCheckNotifier healthCheckNotifier,
    SystemStatusMonitor statusMonitor,
    IOptions<ConsumerConfiguration> queues,
    IOptions<PublisherConfiguration> publisherQueue,
    IOptions<OperatorIdentification> providerIdentification
    ) : BaseRabbitMQWorker(logger, rabbitConnection.CreateConnection(), healthCheckNotifier, statusMonitor, queues.Value.BebopUserTransferRequestQueue)
{

    PublisherConfiguration _publisherQueue = publisherQueue.Value;

    protected override async Task ProcessMessageAsync(BasicDeliverEventArgs eventArgs, IModel channel)
    {
        var body = eventArgs.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        var headers = eventArgs.BasicProperties.Headers;
        var operation = headers.GetUserEventType();
        using var scope = serviceProvider.CreateScope();

        if(operation is UserOperations.TransferUser)
        {
            var userTransferDto = JsonSerializer.Deserialize<UserTransferRequestDto>(message) ?? throw new InvalidBodyException();
            logger.LogInformation("Processing request for user {userId}", userTransferDto.UserId);
            var useCaseSelector = scope.ServiceProvider.GetRequiredService<IUserTransferRequestUseCase>();
            var unRegisterUserDto = await useCaseSelector.TryDeactivateUserAsync(userTransferDto);
            PublishUnregisterUserRequestToProxy(userTransferDto.UserId, unRegisterUserDto, channel);
        }
        var database = scope.ServiceProvider.GetRequiredService<StandUsersDbContext>();
        await database.SaveChangesAsync();
        channel.BasicAck(eventArgs.DeliveryTag, false);
    }

    private void PublishUnregisterUserRequestToProxy(Guid userId, UnRegisterUserDto? unRegisterUserDto, IModel channel)
    {
        if(unRegisterUserDto is null)
        {
            logger.LogWarning("User with Id {id} not found on the system", userId);
            return;
        }
        var requestHeaders = new EventHeaders(UserOperations.UnregisterUser.ToString(), userId);
        var properties = channel.CreateBasicProperties();
        properties.Headers = requestHeaders.GetAttributesAsDictionary();
        string jsonResult = JsonSerializer.Serialize(unRegisterUserDto);
        byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonResult);
        var requestQueue = _publisherQueue.UserRequestQueue;
        channel.BasicPublish("", requestQueue, properties, jsonBytes);
    }
}
