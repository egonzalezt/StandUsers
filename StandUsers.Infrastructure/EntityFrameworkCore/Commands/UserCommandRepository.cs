namespace StandUsers.Infrastructure.EntityFrameworkCore.Commands;

using Domain.User;
using Domain.User.Repositories;
using DbContext;
using Microsoft.EntityFrameworkCore;

internal class UserCommandRepository : IUserCommandRepository
{
    private readonly StandUsersDbContext _context;

    public UserCommandRepository(StandUsersDbContext standUsersDbContext)
    {
        _context = standUsersDbContext;
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByIdAsync(Guid id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByIdentificationNumberAsync(int identificationNumber)
    {
        return await _context.Users.AnyAsync(u => u.IdentificationNumber == identificationNumber);
    }

}
