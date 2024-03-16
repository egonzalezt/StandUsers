﻿namespace StandUsers.Infrastructure.SystemPostProcessor;

using Microsoft.Extensions.Options;
using Application.Interfaces;
using Domain.SharedKernel;
using Domain.User.Dtos;
using Infrastructure.MessageBroker;
using Infrastructure.MessageBroker.Options;
using Infrastructure.MessageBroker.Publisher;
using Microsoft.Extensions.Logging;

internal class UserCreatedPostProcessor(ILogger<UserCreatedPostProcessor> logger, IMessageSender messageSender, IOptions<PublisherConfiguration> publiserOptions) : IPostProcessor<UserOwnedDto>
{
    private readonly PublisherConfiguration _publisherConfiguration = publiserOptions.Value;
    public void NotifyCreation(UserOwnedDto entity, string messageType, NotificationTypes notificationType)
    {
        logger.LogInformation("Sending message on the queue {queue} to notify user own", _publisherConfiguration.UserOwnedBroadcastQueue);
        var headers = new EventHeaders(messageType, entity.Id);
        if (notificationType == NotificationTypes.Broadcast)
        {
            messageSender.SendBroadcast(entity, _publisherConfiguration.UserOwnedBroadcastQueue, headers.GetAttributesAsDictionary());
        }
        else
        {
            messageSender.SendMessage(entity, _publisherConfiguration.UserOwnedBroadcastQueue, headers.GetAttributesAsDictionary());
        }
        logger.LogInformation("Message sent using the publisher mode {mode}", notificationType.ToString());
    }

    public void NotifyDeletion(UserOwnedDto entity, string messageType, NotificationTypes notificationType)
    {
        logger.LogInformation("Sending message on the queue {queue}", _publisherConfiguration.UserOwnedBroadcastQueue);
        var headers = new EventHeaders(messageType, entity.Id);
        if (notificationType == NotificationTypes.Broadcast)
        {
            messageSender.SendBroadcast(entity, _publisherConfiguration.UserNotificationsQueue, headers.GetAttributesAsDictionary());
        }
        else
        {
            messageSender.SendMessage(entity, _publisherConfiguration.UserNotificationsQueue, headers.GetAttributesAsDictionary());
        }
        logger.LogInformation("Message sent using the publisher mode {mode}", notificationType.ToString());
    }
}
