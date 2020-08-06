using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private readonly IUserDao _userDao;
        private readonly TelegramBotClient _client;
        private readonly ICounterDao _counterDao;

        public GetCountersCommand(ICounterDao counterDao, IUserDao userDao, TelegramBotClient client)
        {
            _userDao = userDao;
            _client = client;
            _counterDao = counterDao;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            await GetTopNAndRespond(update, 16);
        }

        private async Task GetTopNAndRespond(Update update, int N)
        {
            var chatId = update.Message.Chat.Id;

            var counters = await _counterDao.GetCountersWithLimit(chatId, N);

            var userCounters =
                await Task.WhenAll(counters.Select(async (c) => new
                {
                    User = await _userDao.GetUserById(c.UserId),
                    Counter = c.Value
                }));

            var result = userCounters
                .Select(uc => (
                    (uc.User != null ? uc.User.FirstName + " " + uc.User.LastName : "%Unknown%").Escape(), 
                    uc.Counter));

            var text = await CreateText(result);

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                text,
                replyToMessageId: update.Message.MessageId,
                parseMode: ParseMode.Html);
        }

        private static Task<string> CreateText(IEnumerable<(string Username, long Counter)> users)
        {
            var text = new StringBuilder();

            var values = users.ToList();

            text.AppendLine($@"Top {values.Count()} counters:");

            var table = TableGenerator.GenerateNumberedList(
                values.OrderByDescending(uc => uc.Counter)
                    .Select(uc => ((object)uc.Username, (object)uc.Counter)));

            text.AppendLine(table);

            return Task.FromResult(text.ToString());
        }
    }
}