using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
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
            var handledBy = new List<string>();
            _logger.LogInformation(
                "New update:\n{Message}",
                JsonConvert.SerializeObject(update, Formatting.Indented)
            );

            try
            {
                foreach (var handler in Handlers)
                {
                    var handleContext = new HandleContext { HandledBy = handledBy };
                    if (await handler.IsHandleable(update, handleContext))
                    {
                        _logger.LogInformation(
                            "Matched with {HandlerType} handler",
                            handler.GetType().Name
                        );
                        var handled = await handler.HandleUpdate(update, handleContext);
                        if (handled)
                        {
                            handledBy.Add(handler.GetType().Name);
                        }
                    }
                }
            }
            catch (ApiRequestException ex)
            {
                _logger.LogError(ex, "Error during routing: {Error}", ex.Message);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during routing: {Error}", ex.Message);
                throw;
            }

            if (!handledBy.Any())
            {
                _logger.LogInformation($"No handlers have handled that message");
            }
        }
    }
}
