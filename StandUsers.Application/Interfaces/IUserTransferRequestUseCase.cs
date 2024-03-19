namespace StandUsers.Application.Interfaces;

using Domain.User.Dtos;
using Domain.Centralizer.Dtos;

public interface IUserTransferRequestUseCase
{
    Task<UnRegisterUserDto?> TryDeactivateUserAsync(UserTransferRequestDto userTransferRequestDto);
}
