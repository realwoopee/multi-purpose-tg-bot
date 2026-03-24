using System.Net;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.APIL.WebApi.Setup;

public static class SetupTelegram
{
    public static IServiceCollection AddTelegramBotClient(
        this IServiceCollection services,
        AppConfiguration appConfig
    )
    {
        TelegramBotClient botClient;

        if (appConfig.UseSocks5)
        {
            var httpClientHandler = new HttpClientHandler
            {
                Proxy = new WebProxy($"socks5://{appConfig.Socks5Host}:{appConfig.Socks5Port}")
                {
                    Credentials = new NetworkCredential(
                        appConfig.Socks5User,
                        appConfig.Socks5Password
                    ),
                },
                UseProxy = true,
            };
            var httpClient = new HttpClient(httpClientHandler);
            botClient = new TelegramBotClient(appConfig.TelegramToken, httpClient);
        }
        else
        {
            botClient = new TelegramBotClient(appConfig.TelegramToken);
        }

        services.AddSingleton(botClient);

        return services;
    }
}
