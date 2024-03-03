namespace StandUsers.Application.Interfaces; 

using Domain.User.Dtos;

public interface ICreateUserUseCase
{
    Task<Guid> ExecuteAsync(UserDto userDto);
}
