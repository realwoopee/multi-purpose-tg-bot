using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class CommandExecutor : IHandler
    {
        private readonly IEnumerable<ICommand> _commands;

        public CommandExecutor(IEnumerable<ICommand> commands)
        {
            _commands = commands;
        }

        public async Task<bool> IsHandleable(Update update) =>
            await Task.Run(() =>
                update.Message?.ForwardFrom == null && update.Message?.ForwardFromChat == null 
                && update.Message?.Text != null 
                && update.Message.Text.StartsWith('/'));

        public async Task<bool> HandleUpdate(Update update)
        {
            var text = update.Message.Text.Substring(1);
            var name = text.Split(' ', '@').First();
            var args = text.Split(' ').Skip(1).ToArray();

            foreach (var command in _commands)
            {
                if (name != command.Name) continue;
                
                await command.Execute(update, name, args);
                return true;
            }

            return false;
        }
    }
}
