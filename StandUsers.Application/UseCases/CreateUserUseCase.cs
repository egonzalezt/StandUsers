namespace StandUsers.Application.UseCases;

using Application.Interfaces;
using StandUsers.Domain.User;
using StandUsers.Domain.User.Dtos;
using StandUsers.Domain.User.Exceptions;
using StandUsers.Domain.User.Repositories;
using System;
using System.Threading.Tasks;

internal class CreateUserUseCase : ICreateUserUseCase
{
    private readonly IUserCommandRepository _userCommandRepository;
    private readonly IUserQueryRepository _userQueryRepository;

    public CreateUserUseCase(
        IUserCommandRepository userCommandRepository, 
        IUserQueryRepository userQueryRepository
    )
    {
        _userCommandRepository = userCommandRepository;
        _userQueryRepository = userQueryRepository;
    }

    public async Task<Guid> ExecuteAsync(UserDto userDto)
    {
        var emailAlreadyExist = await _userCommandRepository.ExistsByEmailAsync( userDto.Email.Value );
        if (emailAlreadyExist)
        {
            throw new EmailAlreadyExists(userDto.Email.Value);
        }
        var identificationNumberAlreadyExists = await _userCommandRepository.ExistsByIdentificationNumberAsync(userDto.IdentificationNumber.Value);
        if(identificationNumberAlreadyExists)
        {
            throw new IdentificationNumberAlreadyExists(userDto.IdentificationNumber.Value);
        }
        var user = User.Build(userDto.Name, userDto.Email.Value, userDto.IdentificationNumber.Value);
        await _userQueryRepository.CreateAsync(user);
        return user.Id;
    }
}
