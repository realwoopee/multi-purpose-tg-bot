using System;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;
using User = WordCounterBot.Common.Entities.User;

namespace WordCounterBot.BLL.Core.Commands
{
    public class GetPersonalStatsCommand : ICommand
    {
        private readonly MessageSender _messageSender;
        private readonly UserResolver _userResolver;
        private readonly UserStatsService _userStatsService;
        public string Name { get; } = @"getpersonalstats";

        public GetPersonalStatsCommand(
            UserResolver userResolver,
            MessageSender messageSender, 
            UserStatsService userStatsService)
        {
            _userResolver = userResolver;
            _messageSender = messageSender;
            _userStatsService = userStatsService;
        }

        public async Task Execute(Update update, string command, params string[] args)
        {
            var user = await _userResolver.ResolveTargetUser(args, update);

            if (user == null)
            {
                await _messageSender.SendHtmlReplyAsync(update, "Пользователя в базе майора не нашли");
                return;
            }
            
            var stats = await _userStatsService.GetUserStatistics(update.GetChatId(), user.Id, update.GetMessageDate());
            
            var statsText = FormatStatsMessage(user, stats);

            await _messageSender.SendHtmlReplyAsync(update, statsText);
        }

        private static string FormatStatsMessage(User user, UserStatistics stats)
        {
            return $"{UserFormatter.FormatUserName(user).HtmlBold()}\n\nА че это за чел?\n\n"
                   + $"Total count - {stats.Total} {"words".HtmlItalic()}.\n\n"
                   + $"Today count - {stats.Today} {"words".HtmlItalic()}.\n"
                   + $"This week count - {stats.Week} {"words".HtmlItalic()}.\n"
                   + $"This month count - {stats.Month} {"words".HtmlItalic()}.\n\n"
                   + $"Last message was on {stats.LastMessageDate:dd MMMM yyyy}\n";
        }
    }
}