using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
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
            await Task.Run(() => update.Message.Text != null && update.Message.Text.StartsWith('/'));

        public async Task HandleUpdate(Update update)
        {
            await Task.CompletedTask;
        }
    }
}
