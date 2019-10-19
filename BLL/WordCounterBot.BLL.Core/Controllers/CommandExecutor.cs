using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class CommandExecutor : IHandler
    {
        private ICounterDao _counterDao;
        private IEnumerable<ICommand> _commands;
        private TelegramBotClient _client;

        public CommandExecutor(IEnumerable<ICommand> commands, TelegramBotClient client, ICounterDao counterDao, IUserDao userDao)
        {
            _counterDao = counterDao;
            _commands = commands;
            _client = client;
        }

        public async Task<bool> Predicate(Update update) =>
            await Task.Run(() =>
                update.Message?.Text != null &&
                update.Message.Text.StartsWith('/'));

        public async Task HandleUpdate(Update update)
        {
            var text = update.Message.Text.Substring(1);
            var name = text.Split(' ', '@').First();
            var args = text.Split(' ').Skip(1).ToArray();

            foreach (var command in _commands)
            {
                if (name == command.Name)
                {
                    await command.Execute(update, name, args);
                    return;
                }
            }
        }
    }
}
