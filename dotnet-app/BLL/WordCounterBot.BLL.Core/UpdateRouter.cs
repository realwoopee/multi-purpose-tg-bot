using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core
{
    public class UpdateRouter : IRouter
    {
        private readonly ILogger _logger;

        public IEnumerable<IHandler> Handlers { get; }

        public UpdateRouter(ILogger<UpdateRouter> logger, IEnumerable<IHandler> handlers)
        {
            _logger = logger;
            Handlers = handlers;
        }

        public async Task Route(Update update)
        {
            var handled = false;

            try
            {
                foreach (var handler in Handlers)
                {
                    _logger.LogInformation(
                        $"Update\n\"{JsonConvert.SerializeObject(update, Formatting.Indented)}\"");
                    if (await handler.IsHandleable(update))
                    {
                        _logger.LogInformation(
                            $"Matched with {handler.GetType()} handler.");
                        handled = await handler.HandleUpdate(update);
                    }
                }
            }
            catch (ApiRequestException ex)
            {
                _logger.LogError(ex,
                    $"Error during routing: {ex.Message};\nUpdate: {JsonConvert.SerializeObject(update, Formatting.Indented)}");
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    $"Error during routing: {ex.Message};\nUpdate: {JsonConvert.SerializeObject(update, Formatting.Indented)}");
                throw;
            }

            if (!handled)
            {
                _logger.LogInformation(
                    $"No handlers have handled that message:\n{JsonConvert.SerializeObject(update, Formatting.Indented)}");
            }
        }
    }
}
