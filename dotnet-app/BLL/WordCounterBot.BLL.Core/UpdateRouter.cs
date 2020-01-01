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
using WordCounterBot.BLL.Core.Controllers;

namespace WordCounterBot.BLL.Core
{
    public class UpdateRouter : IRouter
    {
        private readonly ILogger _logger;
        private readonly UserUpdater _userUpdater;

        public IEnumerable<IHandler> Handlers { get; set; }
        public IHandler DefaultHandler { get; set; }

        public UpdateRouter(ILogger<UpdateRouter> logger, IEnumerable<IHandler> handlers, DefaultHandler defaultHandler, UserUpdater userUpdater)
        {
            _logger = logger;
            Handlers = handlers;
            DefaultHandler = defaultHandler;
            _userUpdater = userUpdater;
        }

        public async Task Route(Update update)
        {
            try
            {
                await _userUpdater.Update(update.Message?.From);
                foreach (var handler in Handlers)
                {
                    if (await handler.IsHandable(update))
                    {
                        _logger.LogInformation(
                            $"Update \"{JsonConvert.SerializeObject(update, Formatting.Indented)}\" matched with {handler.GetType()} handler");
                        await handler.HandleUpdate(update);
                        return;
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
