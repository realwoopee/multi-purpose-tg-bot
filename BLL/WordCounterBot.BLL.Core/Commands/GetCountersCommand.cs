using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Contracts 
{
    public class GetCountersCommand : ICommand
    {
        public string Name { get; } = @"getcounters";

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
            await GetTopNAndRespond(update, 10);
        }

        private async Task GetTopNAndRespond(Update update, int N)
        {
            var counters = await _counterDao.GetCountersWithLimit(update.Message.Chat.Id, N);

            var userId = update.Message.From.Id;

            var userCounters =
                await Task.WhenAll(counters.Select(async (c) => new
                {
                    User = await _userDao.GetUserById(c.userId),
                    Counter = c.counter
                }));

            var result = userCounters.Select(uc => 
                ((object)(uc.User != null ? uc.User.FirstName + " " + uc.User.LastName : "%Unknown%"), (object)uc.Counter)
            ).ToList();

            var table = TableGenerator.GenerateTable("person", "words", result);

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                "<pre>" + table + "</pre>",
                replyToMessageId: update.Message.MessageId,
                parseMode: ParseMode.Html);
        }
    }
}