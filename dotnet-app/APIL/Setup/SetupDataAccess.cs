using Microsoft.Extensions.DependencyInjection;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.DAL.Postgresql;

namespace WordCounterBot.APIL.WebApi.Setup;

public static class SetupDataAccess
{
    public static IServiceCollection AddDataAccessServices(this IServiceCollection services)
    {
        services.AddScoped<ICounterDao, CounterDao>();
        services.AddScoped<ICounterDatedDao, CounterDatedDao>();
        services.AddScoped<IUserDao, UserDaoPostgreSQL>();

        return services;
    }
}
