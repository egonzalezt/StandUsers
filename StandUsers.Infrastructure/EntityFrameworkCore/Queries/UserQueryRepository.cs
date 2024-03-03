namespace StandUsers.Infrastructure.EntityFrameworkCore.Queries;

using Domain.User.Repositories;
using Domain.User;
using DbContext;
using System.Threading.Tasks;

internal class UserQueryRepository : IUserQueryRepository
{
    private readonly StandUsersDbContext _context;

    public UserQueryRepository(StandUsersDbContext context)
    {
        _context = context;
    }
    public async Task CreateAsync(User user)
    {
        await _context.AddAsync(user);
        await _context.SaveChangesAsync();
    }
}
