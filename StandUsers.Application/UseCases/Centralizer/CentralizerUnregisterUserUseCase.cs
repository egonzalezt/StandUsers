namespace StandUsers.Application.UseCases.Centralizer;

using Microsoft.Extensions.Logging;
using Application.Interfaces;
using Domain.Centralizer;
using Domain.SharedKernel;
using Domain.User;
using Domain.User.Dtos;
using Domain.User.Repositories;
using Exceptions;

public class CentralizerUnregisterUserUseCase(IPostProcessor<UserTransferResponseDto> postProcessor, IUserCommandRepository userCommandRepository, IUserQueryRepository userQueryRepository, ILogger<CentralizerUserCreatedUseCase> logger) : ICentralizerUserUseCase
{
    public UserOperations UseCase { get; } = UserOperations.UnregisterUser;

    public async Task ExecuteAsync<T>(GenericResponse<T> response, string userId)
    {
        logger.LogInformation("GovCarpeta answer the request to unregister a new user");
        var id = Guid.Parse(userId);
        var user = await userCommandRepository.GetAsync(id) ?? throw new UserNotFoundException(userId);
        if (response.StatusCode == 200 || response.StatusCode == 201 || response.StatusCode == 204)
        {
            logger.LogInformation("User with Id {id}, is not part of the system, replying request to transfer service", id);
            var userOwned = new UserTransferResponseDto { Email = user.Email, Id = user.Id, IdentificationNumber = user.IdentificationNumber, Name = user.Name, Direction = user.Direction };
            postProcessor.NotifyDeletion(userOwned, UserOperations.StandUserResponse.ToString(), NotificationTypes.Single);
        }
    }
}
