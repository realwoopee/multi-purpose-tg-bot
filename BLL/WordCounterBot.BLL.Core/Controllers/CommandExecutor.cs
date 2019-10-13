using System.Collections.Generic;
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
            await Task.CompletedTask;
        }
    }
}
