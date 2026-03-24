using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Commands
{
    public class GetStatsForLastWeekCommand : ICommand
    {
        public string Name { get; } = @"getstatslastweek";

        private readonly MessageSender _messageSender;
        private readonly LeaderboardService _leaderboardService;

        public GetStatsForLastWeekCommand(
            LeaderboardService leaderboardService,
            MessageSender messageSender)
        {
            _messageSender = messageSender;
            _leaderboardService = leaderboardService;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            var msgDate = update.GetMessageDate();
            var chatId = update.GetChatId();

            var rows = await _leaderboardService.GetWeeklyLeaderboard(chatId, msgDate, limit: 16);

            var text = TableGenerator.GenerateTop(
                $"Top {rows.Count} counters for last 7 days:",
                rows.Select(r => (r.UserName, r.WordCount)));

            await _messageSender.SendHtmlReplyAsync(update, text);
        }
    }
}