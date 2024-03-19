namespace StandUsers.Application.UseCases;

using Application.Interfaces;
using Domain.User.Dtos;
using Domain.User.Repositories;
using Microsoft.Extensions.Options;
using Domain.Centralizer.Dtos;
using Domain.SharedKernel;

internal class UserTransferRequestUseCase(
    IUserCommandRepository userCommandRepository, 
    IUserQueryRepository userQueryRepository, 
    IOptions<OperatorIdentification> providerIdentification
) : IUserTransferRequestUseCase
{
    OperatorIdentification _providerIdentification = providerIdentification.Value;

    public async Task<UnRegisterUserDto?> TryDeactivateUserAsync(UserTransferRequestDto userTransferRequestDto)
    {
        var user = await userCommandRepository.GetAsync(userTransferRequestDto.UserId);
        if (user is null)
        {
            return null;
        }
        user.DeActivateUser();
        return new UnRegisterUserDto
        {
            Id = user.IdentificationNumber,
            OperatorId = _providerIdentification.Uid,
            OperatorName = _providerIdentification.Name
        };
    }
}
