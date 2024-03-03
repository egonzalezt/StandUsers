namespace StandUsers.Infrastructure.EntityFrameworkCore;

using Microsoft.Extensions.DependencyInjection;
using Domain.User.Repositories;
using Commands;
using Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using DbContext;

internal static class ConfigureEntityFramework
{
    public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<StandUsersDbContext>(options => {
            options.UseNpgsql(configuration.GetConnectionString("PostgresSql"));
        });
    }

    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserCommandRepository, UserCommandRepository>();
        services.AddScoped<IUserQueryRepository, UserQueryRepository>();
    }
}
