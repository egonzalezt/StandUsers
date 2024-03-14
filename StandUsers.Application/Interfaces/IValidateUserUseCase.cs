namespace StandUsers.Application.Interfaces; 

public interface IValidateUserUseCase
{
    Task<bool> EmailExistsAsync(string email);
    Task<bool> IdentificationNumberExistsAsync(long identificationNumber);
}
