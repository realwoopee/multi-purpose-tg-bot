using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.BLL.Core.Controllers;

namespace WordCounterBot.BLL.Core
{
    public class UpdateRouter : IRouter
    {
        private readonly ILogger _logger;

        public IEnumerable<IHandler> Handlers { get; set; }
        public IHandler DefaultHandler { get; set; }

        public UpdateRouter(ILogger<UpdateRouter> logger, IEnumerable<IHandler> handlers, DefaultHandler defaultHandler)
        {
            _logger = logger;
            Handlers = handlers;
            DefaultHandler = defaultHandler;
        }

        public async Task Route(Update update)
        {
            try
            {
                foreach (var handler in Handlers)
                {
                    if (await handler.Predicate(update))
                    {
                        await handler.HandleUpdate(update);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error during routing: {ex.Message};\nUpdate: {update}");
                throw;
            }
            //No filtered controller has matched

            if (DefaultHandler != null)
            {
                await DefaultHandler.HandleUpdate(update);
                return;
            }
            //No default controller

            _logger.LogError($"Error during routing: Controller for that update was not found. Default controller is not specified.");
            throw new InvalidOperationException("Controller for that update was not found. Default controller is not specified.");
        }
    }
}
