namespace StandUsers.Domain.User.Repositories;

public interface IUserCommandRepository
{
    Task<User?> GetAsync(Guid Id);
    Task<bool> ExistsByIdAsync(Guid id); 
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByIdentificationNumberAsync(int identificationNumber);

}
