using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Core;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.Common.Logging;

namespace WordCounterBot.APIL.WebApi.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private IRouter _router;
        private ILogger _logger;

        public BotController(IRouter router, ILogger logger)
        {
            _router = router;
            _logger = logger;
        }

        [HttpPost("/api/update")]
        public async Task<IActionResult> Update([FromBody]Update update)
        {
            try
            {
                await _router.Route(update);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogException(ex.Message);
                _logger.LogData(update);
                throw;
            }
        }
    }
}