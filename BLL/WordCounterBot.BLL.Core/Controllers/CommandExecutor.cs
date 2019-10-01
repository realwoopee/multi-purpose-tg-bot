using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class CommandExecutor : IHandler
    {
        private Dictionary<string, ICommand> _commands;
        public async Task HandleUpdate(Update update)
        {
            throw new NotImplementedException();
        }
    }
}
