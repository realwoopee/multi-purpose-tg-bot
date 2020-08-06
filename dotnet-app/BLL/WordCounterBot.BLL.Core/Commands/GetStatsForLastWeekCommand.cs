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
    public class GetStatsForLastWeekCommand : ICommand
    {
        public string Name { get; } = @"getstatslastweek";

        private readonly IUserDao _userDao;
        private readonly TelegramBotClient _client;
        private readonly ICounterDatedDao _counterDatedDao;

        public GetStatsForLastWeekCommand(ICounterDatedDao counterDatedDao, IUserDao userDao, TelegramBotClient client)
        {
            _userDao = userDao;
            _client = client;
            _counterDatedDao = counterDatedDao;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            await GetTopNAndRespond(update, 16);
        }

        private async Task GetTopNAndRespond(Update update, int N)
        {
            var msgDate = update.Message.Date.Date;
            var chatId = update.Message.Chat.Id;
            var dateLimit = TimeSpan.FromDays(7);

            var counters = await _counterDatedDao.GetCounters(chatId, msgDate, dateLimit, N);

            var countersSummed = counters.GroupBy(c => (c.ChatId, c.UserId))
                                         .Select(g => (g.Key, g.Sum(c => c.Value)));

            var userCounters =
                await Task.WhenAll(countersSummed.Select(async (c) => new
                {
                    User = await _userDao.GetUserById(c.Key.UserId),
                    Counter = c.Item2
                }));

            var result = userCounters
                .Select(uc => (
                    (uc.User != null ? uc.User.FirstName + " " + uc.User.LastName : "%Unknown%").Escape(), 
                    uc.Counter));

            var text = CreateText(result);

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                text,
                replyToMessageId: update.Message.MessageId,
                parseMode: ParseMode.Html);
        }

        private static string CreateText(IEnumerable<(string Username, long Counter)> users)
        {
            var text = new StringBuilder();

            var values = users.ToList();

            text.AppendLine($@"Top {values.Count()} counters for 7 days:");

            var table = TableGenerator.GenerateNumberedList(
                values.OrderByDescending(uc => uc.Counter)
                    .Select(uc => ((object)uc.Username, (object)uc.Counter)));

            text.AppendLine(table);

            return text.ToString();
        }
    }
}