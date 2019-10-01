using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class DefaultController : IController
    {
        public Task HandleUpdate(Update update)
        {
            return Task.CompletedTask;
        }
    }
}
