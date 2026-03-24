using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.APIL.WebApi.Setup;

public static class SetupWebhook
{
    public static IApplicationBuilder UseTelegramWebhook(
        this IApplicationBuilder app,
        IWebHostEnvironment env,
        ILogger logger)
    {
        var botClient = app.ApplicationServices.GetRequiredService<TelegramBotClient>();
        var appConfig = app.ApplicationServices.GetRequiredService<AppConfiguration>();

        _ = appConfig.IsSSLCertSelfSigned
            ? SetWebhookWithCertificateAsync(botClient, appConfig, env, logger)
            : SetWebhookWithoutCertificateAsync(botClient, appConfig, logger);

        logger.LogInformation(
            "Configured HTTP pipeline. AppSettings is {Config}",
            JsonConvert.SerializeObject(appConfig, Formatting.Indented)
        );

        return app;
    }

    private static async Task SetWebhookWithCertificateAsync(
        TelegramBotClient botClient,
        AppConfiguration appConfig,
        IWebHostEnvironment env,
        ILogger logger)
    {
        try
        {
            var certFileInfo = env.ContentRootFileProvider.GetFileInfo(appConfig.SSLCertPath);
            await using var stream = certFileInfo.CreateReadStream();
            var sslCert = new InputFileStream(stream);

            await botClient.DeleteWebhookAsync();
            await botClient.SetWebhookAsync(appConfig.WebhookUrl.ToString(), sslCert);

            logger.LogInformation(
                "Set webhook to {Url}, SSL cert is {Cert}",
                appConfig.WebhookUrl,
                certFileInfo.Name
            );
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set webhook with certificate");
        }
    }


    private static async Task SetWebhookWithoutCertificateAsync(
        TelegramBotClient botClient,
        AppConfiguration appConfig,
        ILogger logger)
    {
        try
        {
            await botClient.DeleteWebhookAsync();
            await botClient.SetWebhookAsync(appConfig.WebhookUrl.ToString());

            logger.LogInformation("Set webhook to {Url}", appConfig.WebhookUrl);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to set webhook");
        }
    }
}