using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Common.Helpers;
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
            await Task.Run(() =>
                update.Message is not null
                && update.Message.IsNotForwarded()
                && (
                    update.Message.Entities?.Any(x => x.Type == MessageEntityType.BotCommand)
                    ?? false
                )
            );

        public async Task<bool> HandleUpdate(Update update, HandleContext context)
        {
            var commandEntities = update.Message.Entities?
                .Where(x => x.Type == MessageEntityType.BotCommand).ToList();

            if (commandEntities == null) return false;

            var handled = false;
            var lastProcessedOffset = 0;

            foreach (var entity in commandEntities.Where(entity => entity.Offset >= lastProcessedOffset))
            {
                var (name, args) = ExtractCommandData(update.Message.Text, entity);
                var command = _commands.FirstOrDefault(c => c.Name == name);

                if (command != null)
                {
                    await command.Execute(update, name, args);
                    handled = true;
                }

                lastProcessedOffset = GetNextLineOffset(update.Message.Text, entity.Offset);
            }

            return handled;
        }

        private static (string Name, string[] Args) ExtractCommandData(string text, MessageEntity entity)
        {
            var commandPart = text[(entity.Offset + 1)..];
            var parts = commandPart.Split(' ');

            var name = parts[0].Split('@').First().ToLowerInvariant();
            var args = parts.Skip(1).ToArray();

            return (name, args);
        }

        private static int GetNextLineOffset(string text, int offset)
        {
            var nextLineIndex = text.IndexOf('\n', offset);
            return nextLineIndex == -1 ? text.Length : nextLineIndex;
        }
    }
}
