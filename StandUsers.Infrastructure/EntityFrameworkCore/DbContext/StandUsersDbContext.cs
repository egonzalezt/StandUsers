namespace StandUsers.Infrastructure.EntityFrameworkCore.DbContext;

using Microsoft.EntityFrameworkCore;
using Domain.User;
using Infrastructure.EntityFrameworkCore.DbContext.ModelBuilders;

public class StandUsersDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public StandUsersDbContext(DbContextOptions<StandUsersDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Configure();
    }
}
