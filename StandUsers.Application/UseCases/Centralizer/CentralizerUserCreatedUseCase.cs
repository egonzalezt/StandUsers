namespace StandUsers.Application.UseCases.Centralizer;

using Domain.Centralizer;
using Exceptions;
using Domain.User;
using Domain.User.Repositories;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Domain.User.Dtos;
using Domain.SharedKernel;

public class CentralizerUserCreatedUseCase(IPostProcessor<UserOwnedDto> postProcessor, IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository, ILogger<CentralizerUserCreatedUseCase> logger) : ICentralizerUserUseCase
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
            var userOwned = new UserOwnedDto { Email = user.Email, Id = user.Id, IdentificationNumber = user.IdentificationNumber, Name = user.Name };
            postProcessor.NotifyCreation(userOwned, UserOperations.CreateUser.ToString(), NotificationTypes.Broadcast);
        }
        if(response.StatusCode == 501)
        {
            logger.LogInformation("User with Id {id}, is on other system", id);
            userQueryRepository.Delete(user);
            var userOwned = new UserOwnedDto { Email = user.Email, Id = user.Id, IdentificationNumber = user.IdentificationNumber, Name = user.Name };
            postProcessor.NotifyDeletion(userOwned, UserOperations.ExistsOnOtherProvider.ToString(), NotificationTypes.Single);
        }
    }
}
