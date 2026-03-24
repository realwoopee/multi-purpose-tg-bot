using Microsoft.Extensions.DependencyInjection;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.BLL.Core;
using WordCounterBot.BLL.Core.Commands;
using WordCounterBot.BLL.Core.Controllers;

namespace WordCounterBot.APIL.WebApi.Setup;

public static class SetupBotServices
{
    public static IServiceCollection AddBotServices(this IServiceCollection services)
    {
        // Services
        services.AddScoped<MessageSender>();
        services.AddScoped<UserResolver>();
        services.AddScoped<UserStatistics>();
        services.AddScoped<LeaderboardRow>();
        services.AddScoped<UserStatsService>();
        services.AddScoped<LeaderboardService>();
        
        // Commands
        services.AddTransient<ICommand, GetPersonalStatsCommand>();
        services.AddTransient<ICommand, GetCountersCommand>();
        services.AddTransient<ICommand, GetStatsForCurrentDayCommand>();
        services.AddTransient<ICommand, GetStatsForLastWeekCommand>();

        // Handlers
        services.AddTransient<IHandler, CommandExecutor>();
        services.AddTransient<IHandler, SystemMessageHandler>();
        services.AddTransient<IHandler, WordCounter>();
        services.AddTransient<IHandler, UserInfoHandler>();
        services.AddTransient<IHandler, StringReplacer>();

        // Router
        services.AddScoped<IRouter, UpdateRouter>();

        // Caching
        services.AddMemoryCache();
        services.AddTransient<MemoryMessageStorage>();

        return services;
    }
}
