using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WordCounterBot.Common.Entities;
using WordCounterBot.Common.Logging;

namespace WordCounterBot.APIL.WebApi.Setup;

public static class SetupLogging
{
    public static IServiceCollection AddTelegramLogging(
        this IServiceCollection services,
        AppConfiguration appConfig)
    {
        services.AddLogging(builder =>
        {
            builder.AddProvider(
                new TelegramMessengerLoggerProvider(
                    new TelegramMessengerLoggerConfiguration
                    {
                        LogLevel = LogLevel.Warning,
                        TelegramToken = appConfig.TelegramToken,
                        UserId = appConfig.UserIdForLogger,
                        UseSocks5 = appConfig.UseSocks5,
                        Socks5Host = appConfig.Socks5Host,
                        Socks5Port = appConfig.Socks5Port
                    }
                )
            );

            builder.AddConsole(options =>
                options.LogToStandardErrorThreshold = LogLevel.Trace);
        });

        return services;
    }
}
