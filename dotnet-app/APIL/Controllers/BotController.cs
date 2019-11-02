using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;
using Microsoft.Extensions.Logging;

namespace WordCounterBot.APIL.WebApi.Controllers
{
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IRouter _router;
        private readonly ILogger<BotController> _logger;

        public BotController(IRouter router, ILogger<BotController> logger)
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
                _logger.LogError(ex, $"Error during processing update with BotController: {ex.Message};\nUpdate: {update}");
                throw;
            }
        }
    }
}