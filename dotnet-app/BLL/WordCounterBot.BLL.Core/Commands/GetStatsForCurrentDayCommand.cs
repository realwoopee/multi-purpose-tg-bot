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
    public class GetStatsForCurrentDayCommand : ICommand
    {
        public string Name { get; } = @"getstatscurrentday";

        private readonly IUserDao _userDao;
        private readonly TelegramBotClient _client;
        private readonly ICounterDatedDao _counterDatedDao;

        public GetStatsForCurrentDayCommand(ICounterDatedDao counterDatedDao, IUserDao userDao, TelegramBotClient client)
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
            var date = update.Message.Date.Date;
            var counters = await _counterDatedDao.GetCounters(update.Message.Chat.Id, date, N);

            N = Math.Min(counters.Count(), N);

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

            var text = CreateText(result, date);

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                text,
                replyToMessageId: update.Message.MessageId,
                parseMode: ParseMode.Html);
        }

        private static string CreateText(IEnumerable<(string Username, long Counter)> users, DateTime date)
        {
            var text = new StringBuilder();

            var values = users.ToList();

            text.AppendLine($@"Top {values.Count()} counters for {date:dd MMMM yyyy}:");

            var table = TableGenerator.GenerateNumberedList(
                values.OrderByDescending(uc => uc.Counter)
                    .Select(uc => ((object)uc.Username, (object)uc.Counter)));

            text.AppendLine(table);

            return text.ToString();
        }
    }
}