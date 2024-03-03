namespace StandUsers.Application.Services.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StandUsers.Application.Interfaces;
using StandUsers.Application.UseCases;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IValidateUserUseCase, ValidateUserUseCase>();
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
    }
}
