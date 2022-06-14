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

            _logger.LogInformation("New update:\n{Message}", JsonConvert.SerializeObject(update, Formatting.Indented));
            
            try
            {
                foreach (var handler in Handlers)
                {
                    if (await handler.IsHandleable(update))
                    {
                        _logger.LogInformation(
                            "Matched with {HandlerType} handler", handler.GetType().Name);
                        handled = await handler.HandleUpdate(update);
                    }
                }
            }
            catch (ApiRequestException ex)
            {
                _logger.LogError(ex,
                    "Error during routing: {Error}", ex.Message);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Error during routing: {Error}", ex.Message);
                throw;
            }

            if (!handled)
            {
                _logger.LogInformation(
                    $"No handlers have handled that message");
            }
        }
    }
}
