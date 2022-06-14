using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
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
                && (update.Message?.Entities.Any(x => x.Type == MessageEntityType.BotCommand) ?? false));

        public async Task<bool> HandleUpdate(Update update)
        {
            var text = update.Message.Text.Substring(1);
            var name = text.Split(' ', '@').First().ToLowerInvariant();
            var args = text.Split(' ').Skip(1).ToArray();

            var command = _commands.FirstOrDefault(c => c.Name == name);

            if (command is null) return false;
            
            await command.Execute(update, name, args);
            
            return true;

        }
    }
}
