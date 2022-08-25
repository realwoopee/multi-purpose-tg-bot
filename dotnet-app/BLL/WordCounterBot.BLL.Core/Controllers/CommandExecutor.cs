using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
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

        public async Task<bool> IsHandleable(Update update, HandleContext context) =>
            await Task.Run(
                () =>
                    update.Message?.ForwardFrom == null
                    && update.Message?.ForwardFromChat == null
                    && (
                        update.Message?.Entities?.Any(x => x.Type == MessageEntityType.BotCommand)
                        ?? false
                    )
            );

        public async Task<bool> HandleUpdate(Update update, HandleContext context)
        {
            var commandsEntities = update.Message.Entities
                .Where(x => x.Type == MessageEntityType.BotCommand)
                .ToList();
            int lastEnd = 0;
            var handled = false;
            foreach (var commandEntity in commandsEntities)
            {
                if (commandEntity.Offset < lastEnd)
                    continue;

                //offset is at '/'
                var commandText = update.Message.Text.Substring(commandEntity.Offset + 1);

                var name = commandText.Split(' ', '@').First().ToLowerInvariant();
                var args = commandText.Split(' ').Skip(1).ToArray();

                var command = _commands.FirstOrDefault(c => c.Name == name);

                if (command is not null)
                {
                    await command.Execute(update, name, args);
                    handled = true;
                }
                ;

                lastEnd = update.Message.Text.IndexOf('\n', commandEntity.Offset);
            }
            return handled;
        }
    }
}
