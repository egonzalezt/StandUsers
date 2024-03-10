namespace StandUsers.Application.UseCases;

using Application.Interfaces;
using Domain.User;
using Domain.User.Dtos;
using Domain.User.Exceptions;
using Domain.User.Repositories;
using System.Threading.Tasks;

internal class CreateUserUseCase(
    IUserCommandRepository userCommandRepository,
    IUserQueryRepository userQueryRepository
    ) : ICreateUserUseCase
{
    public async Task<User> ExecuteAsync(UserDto userDto)
    {
        var emailAlreadyExist = await userCommandRepository.ExistsByEmailAsync( userDto.Email );
        if (emailAlreadyExist)
        {
            throw new EmailAlreadyExists(userDto.Email);
        }
        var identificationNumberAlreadyExists = await userCommandRepository.ExistsByIdentificationNumberAsync(userDto.IdentificationNumber);
        if(identificationNumberAlreadyExists)
        {
            throw new IdentificationNumberAlreadyExists(userDto.IdentificationNumber.ToString());
        }
        var user = User.Build(userDto);
        await userQueryRepository.CreateAsync(user);
        return user;
    }
}
