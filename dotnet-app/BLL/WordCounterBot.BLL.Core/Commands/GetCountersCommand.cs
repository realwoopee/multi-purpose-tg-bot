using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Commands
{
    public class GetCountersCommand : ICommand
    {
        public string Name { get; } = @"getcounters";

        private readonly MessageSender _messageSender;
        private readonly LeaderboardService _leaderboardService;

        public GetCountersCommand(
            LeaderboardService leaderboardService,
            MessageSender messageSender)
        {
            _messageSender = messageSender;
            _leaderboardService = leaderboardService;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            var chatId = update.GetChatId();
            var rows = await _leaderboardService.GetAllTimeLeaderboard(chatId, limit: 16);
            
            var text = TableGenerator.GenerateTop(
                $"Top {rows.Count} counters:",
                rows.Select(r => (r.UserName, r.WordCount)));

            await _messageSender.SendHtmlReplyAsync(update, text);
        }
    }
}