namespace StandUsers.Application.UseCases.Centralizer;

using Domain.Centralizer;
using Exceptions;
using Domain.User;
using Domain.User.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using StandUsers.Application.Interfaces;
using StandUsers.Domain.User.Dtos;
using StandUsers.Domain.SharedKernel;

public class CentralizerUserCreatedUseCase(IPostProcessor<UserOwnedDto> postProcessor, IUserCommandRepository userCommandRepository, ILogger<CentralizerUserCreatedUseCase> logger) : ICentralizerUserUseCase
{
    public UserOperations UseCase { get; } = UserOperations.CreateUser;

    public async Task ExecuteAsync<T>(GenericResponse<T> response, string userId)
    {
        logger.LogInformation("GovCarpeta answer the request to create a new user");
        var id = Guid.Parse(userId);
        var user = await userCommandRepository.GetAsync(id) ?? throw new UserNotFoundException(userId);
        if(response.StatusCode == 201)
        {
            logger.LogInformation("User with Id {id}, is now part of the system", id);
            user.ActivateUserByGovCarpeta();
            user.Activate();
            logger.LogInformation("User is now part of the system sending Broadcast message");
            var userOwned = new UserOwnedDto { Email = user.Email, Id = user.Id, IdentificationNumber = user.IdentificationNumber };
            postProcessor.NotifyCreation(userOwned, UserOperations.CreateUser.ToString(), NotificationTypes.Broadcast);
        }
    }
}
