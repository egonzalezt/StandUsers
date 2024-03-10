namespace StandUsers.Application.Interfaces; 

using Domain.User.Dtos;
using Domain.User;

public interface ICreateUserUseCase
{
    Task<User> ExecuteAsync(UserDto userDto);
}
