namespace StandUsers.Application.Services.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces;
using Application.UseCases;
using Domain.Centralizer;

public static class ConfigureApplication
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddCentralizerUseCases();
        services.AddScoped<IValidateUserUseCase, ValidateUserUseCase>();
        services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
        services.AddScoped<IUserTransferRequestUseCase, UserTransferRequestUseCase>();
    }

    public static void AddCentralizerUseCases(this IServiceCollection services)
    {
        typeof(ConfigureApplication).Assembly
            .GetTypes()
            .Where(uc =>
                uc.GetInterfaces().Any(i => i.Name == nameof(ICentralizerUserUseCase)))
            .ToList()
            .ForEach(useCase =>
            {
                var serviceType = useCase.GetInterfaces().First(i => i.Name == nameof(ICentralizerUserUseCase));
                services.AddScoped(serviceType, useCase);
            });
    }
}
