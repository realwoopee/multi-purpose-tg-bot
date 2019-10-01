using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.Common.Logging;

namespace WordCounterBot.BLL.Core
{
    public class UpdateRouter : IRouter
    {
        private ILogger _logger;

        public List<(IFilter, IHandler)> Handlers { get; set; }
        public IHandler DefaultHandler { get; set; }

        public UpdateRouter(ILogger logger)
        {
            _logger = logger;
        }

        public async Task Route(Update update)
        {
            try
            {
                foreach (var tuple in Handlers)
                {
                    var filter = tuple.Item1;
                    var handler = tuple.Item2;
                    if (filter.Predicate(update))
                    {
                        await handler.HandleUpdate(update);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"Error during routing: {ex.Message}");
                throw;
            }
            //No filtered controller has matched

            if (DefaultHandler != null)
            {
                await DefaultHandler.HandleUpdate(update);
                return;
            }
            //No default controller

            _logger.Log($"Error during routing: Controller for that update was not found. Default controller is not specified.");
            throw new InvalidOperationException("Controller for that update was not found. Default controller is not specified.");
        }
    }
}
