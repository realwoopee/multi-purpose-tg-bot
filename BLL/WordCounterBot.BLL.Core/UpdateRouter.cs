using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core
{
    public class UpdateRouter : IRouter
    {
        private readonly ILogger _logger;

        public List<(IFilter, IHandler)> Handlers { get; set; }
        public IHandler DefaultHandler { get; set; }

        public UpdateRouter(ILogger<UpdateRouter> logger)
        {
            _logger = logger;
        }

        public async Task Route(Update update)
        {
            try
            {
                foreach (var (filter, handler) in Handlers)
                {
                    if (filter.Predicate(update))
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
