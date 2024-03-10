﻿namespace StandUsers.Infrastructure.Services.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EntityFrameworkCore;
using Application.Interfaces;
using Domain.User.Dtos;
using Infrastructure.SystemPostProcessor;
using Infrastructure.MessageBroker.Publisher;

public static class ConfigureServices
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEntityFramework(configuration);
        services.AddRepositories();
        services.AddSingleton<IPostProcessor<UserOwnedDto>, UserCreatedPostProcessor>();
        services.AddSingleton<IMessageSender, RabbitMQMessageSender>();
    }
}
