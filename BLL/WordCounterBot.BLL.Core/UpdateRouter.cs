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
        private List<(IFilter, IController)> _controllers;
        private IController _defaultController;
        private ILogger _logger;

        public UpdateRouter(RouterConfig config, ILogger logger)
        {
            _controllers = config.Controllers;
            _defaultController = config.DefaultController;
            _logger = logger;
        }

        public async Task Route(Update update)
        {
            try
            {
                foreach (var tuple in _controllers)
                {
                    var func = tuple.Item1;
                    var controller = tuple.Item2;
                    if (func.Predicate(update))
                    {
                        await controller.HandleUpdate(update);
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

            if (_defaultController != null)
            {
                await _defaultController.HandleUpdate(update);
                return;
            }
            //No default controller

            _logger.Log($"Error during routing: Controller for that update was not found. Default controller is not specified.");
            throw new InvalidOperationException("Controller for that update was not found. Default controller is not specified.");
        }
    }
}
