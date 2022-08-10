
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WordCounterBot.BLL.Core.Controllers;

namespace WordCounterBot.APIL.WebApi.Controllers
{
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly StatusService _serivce;
        private readonly ILogger<StatusController> _logger;

        public StatusController(StatusService serivce, ILogger<StatusController> logger)
        {
            _serivce = serivce;
            _logger = logger;
        }

        [HttpGet("/api/total-count")]
        [HttpHead("/api/total-count")]
        public async Task<IActionResult> Status()
        {
            if (Request.Method == "HEAD")
            {
                return new OkResult();
            }
            
            return new JsonResult(new
            {
                schemaVersion = 1,
                label = "Total word count",
                message = (await _serivce.GetTotalWords()).ToString(),
                color = "blue"
            });
        }

        [HttpGet("/api/ping")]
        [HttpHead("/api/ping")]
        public async Task<IActionResult> Ping()
        {
            return new OkResult();
        }
    }
}