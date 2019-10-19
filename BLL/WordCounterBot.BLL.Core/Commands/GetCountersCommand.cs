using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Contracts 
{
    public class GetCountersCommand : ICommand
    {
        public string Name { get; } = @"get_top";

        private IUserDao _userDao;
        private TelegramBotClient _client;
        private ICounterDao _counterDao;

        public GetCountersCommand(ICounterDao counterDao, IUserDao userDao, TelegramBotClient client)
        {
            _userDao = userDao;
            _client = client;
            _counterDao = counterDao;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            switch (command)
            {
                case "get_top_10":
                    await GetTopNAndRespond(update, 10);
                    break;
                case "get_top_n":
                    if (!int.TryParse(args[0], out var arg))
                    {
                        await  _client.SendTextMessageAsync(
                            update.Message.Chat.Id,
                            "Command argument is not a number",
                            replyToMessageId: update.Message.MessageId);
                    }
                    await GetTopNAndRespond(update, arg);
                    break;
            }
        }

        private async Task GetTopNAndRespond(Update update, int N)
        {
            var counters = await _counterDao.GetCountersWithLimit(update.Message.Chat.Id, N);

            var userId = update.Message.From.Id;

            await _userDao.UpdateUser(update.Message.From);

            var userCounters =
                await Task.WhenAll(counters.Select(async (c) => new
                {
                    User = await _userDao.GetUserById(userId),
                    Counter = c.counter
                }));

            var result = userCounters.Select(uc => 
                ((object)(uc.User.FirstName + " " + uc.User.LastName), (object)uc.Counter)
            ).ToList();

            var table = TableGenerator.GenerateTable("user name", "words", result);

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                table,
                replyToMessageId: update.Message.MessageId);
        }
    }
}