namespace StandUsers.Domain.Centralizer;

using User;

public interface ICentralizerUserUseCase
{
    UserOperations UseCase { get; }

    public Task ExecuteAsync<T>(GenericResponse<T> response, string userId);
}
