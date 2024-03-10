namespace StandUsers.Application.UseCases.Centralizer;

using Domain.Centralizer;
using Exceptions;
using Domain.User;
using Domain.User.Repositories;
using System.Threading.Tasks;

public class CentralizerUserCreatedUseCase : ICentralizerUserUseCase
{
    public UserOperations UseCase { get; } = UserOperations.CreateUser;
    private readonly IUserCommandRepository _userCommandRepository;

    public CentralizerUserCreatedUseCase(IUserCommandRepository userCommandRepository)
    {
        _userCommandRepository = userCommandRepository;
    }

    public async Task ExecuteAsync<T>(GenericResponse<T> response, string userId)
    {
        var id = Guid.Parse(userId);
        var user = await _userCommandRepository.GetAsync(id) ?? throw new UserNotFoundException(userId);
        if(response.StatusCode == 201)
        {
            user.ActivateUserByGovCarpeta();
            user.Activate();
        }
    }
}
