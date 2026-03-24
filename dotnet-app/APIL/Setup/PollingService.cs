using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.APIL.WebApi.Setup;

public class PollingService : BackgroundService
{
    private readonly TelegramBotClient _botClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PollingService> _logger;

    public PollingService(
        TelegramBotClient botClient,
        IServiceProvider serviceProvider,
        ILogger<PollingService> logger)
    {
        _botClient = botClient;
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _botClient.DeleteWebhookAsync(cancellationToken: stoppingToken);
        _logger.LogInformation("Polling mode: webhook deleted, starting long polling");

        int offset = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            Update[] updates;

            try
            {
                updates = await _botClient.GetUpdatesAsync(
                    offset: offset,
                    timeout: 30,
                    cancellationToken: stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching updates");
                await Task.Delay(5000, stoppingToken);
                continue;
            }

            foreach (var update in updates)
            {
                offset = update.Id + 1;

                using var scope = _serviceProvider.CreateScope();
                var router = scope.ServiceProvider.GetRequiredService<IRouter>();
                try
                {
                    await router.Route(update);
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        ex,
                        "Error processing update {UpdateId}: {Message};\nUpdate: {Update}",
                        update.Id,
                        ex.Message,
                        JsonConvert.SerializeObject(update, Formatting.Indented)
                    );
                }
            }
        }
    }
}
