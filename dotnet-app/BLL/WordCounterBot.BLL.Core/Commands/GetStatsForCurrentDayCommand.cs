using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Commands
{
    public class GetStatsForCurrentDayCommand : ICommand
    {
        public string Name { get; } = @"getstatscurrentday";

        private readonly MessageSender _messageSender;
        private readonly LeaderboardService _leaderboardService;

        public GetStatsForCurrentDayCommand(
            LeaderboardService leaderboardService,
            MessageSender messageSender)
        {
            _messageSender = messageSender;
            _leaderboardService = leaderboardService;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            var date = update.GetMessageDate();
            var chatId = update.GetChatId();
            
            var rows = await _leaderboardService.GetDailyLeaderboard(chatId, date, limit: 16);

            var text = TableGenerator.GenerateTop(
                $"Top {rows.Count} counters for {date:dd MMMM yyyy}:",
                rows.Select(r => (r.UserName, r.WordCount)));

            await _messageSender.SendHtmlReplyAsync(update, text);
        }
    }
}