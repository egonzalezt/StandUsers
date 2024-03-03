namespace StandUsers.Application.UseCases;

using Application.Interfaces;
using StandUsers.Domain.User.Repositories;
using System.Threading.Tasks;

internal class ValidateUserUseCase : IValidateUserUseCase
{
    private readonly IUserCommandRepository _userCommandRepository;

    public ValidateUserUseCase(IUserCommandRepository userCommandRepository)
    {
        _userCommandRepository = userCommandRepository;
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _userCommandRepository.ExistsByEmailAsync(email);
    }

    public async Task<bool> IdentificationNumberExistsAsync(string identificationNumber)
    {
        return await _userCommandRepository.ExistsByIdentificationNumberAsync(identificationNumber);
    }
}
