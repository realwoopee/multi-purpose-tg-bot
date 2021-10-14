using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WordCounterBot.DAL.Contracts;
using User = WordCounterBot.Common.Entities.User;

namespace WordCounterBot.BLL.Contracts
{
    public class GetPersonalStatsCommand : ICommand
    {
        private readonly ICounterDao _counterDao;
        private readonly ICounterDatedDao _counterDatedDao;
        private readonly IUserDao _userDao;
        private readonly TelegramBotClient _client;
        public string Name { get; } = @"getpersonalstats";

        public GetPersonalStatsCommand(ICounterDao counterDao, ICounterDatedDao counterDatedDao, IUserDao userDao, TelegramBotClient client)
        {
            _counterDao = counterDao;
            _counterDatedDao = counterDatedDao;
            _userDao = userDao;
            _client = client;
        }
        
        public async Task Execute(Update update, string command, params string[] args)
        {
            string text = "";
            
            if (args.Length > 0)
            {
                text = await GetStats(args[0].TrimStart('@'), update);
            }
            else
            {
                if (update.Message.ReplyToMessage != null)
                {
                    text = await GetStats(update.Message.ReplyToMessage.From.Id, update);
                }
                else
                {
                    text = await GetStats(update.Message.From.Id, update);
                }
            }

            await _client.SendTextMessageAsync(
                update.Message.Chat.Id,
                text,
                replyToMessageId: update.Message.MessageId,
                parseMode: ParseMode.Html);
        }

        private async Task<string> GetStats(long userId, Update update)
        {
            var user = await _userDao.GetUserById(userId);
            if (user == null)
                return "Пользователя в базе майора не нашли";

            return await GetStats(user, update);
        }

        private async Task<string> GetStats(string username, Update update)
        {
            var user = await _userDao.GetUserByUserName(username);
            if (user == null)
                return "Пользователя в базе майора не нашли";

            return await GetStats(user, update);
        }
        
        private async Task<string> GetStats(User user, Update update)
        {
            var msgDate = update.Message.Date.Date;
            var chatId = update.Message.Chat.Id;
            
            var weekSpan = TimeSpan.FromDays(6);
            var monthSpan = TimeSpan.FromDays(30);

            var totalCounter = (await _counterDao.GetPersonalCounter(chatId, user.Id)).Value;
            var todayCounter = (await _counterDatedDao.GetPersonalCounters(chatId, user.Id, msgDate))
                .Sum(c => c.Value);
            var weekCounter = (await _counterDatedDao.GetPersonalCounters(chatId, user.Id, msgDate - weekSpan, msgDate))
                .Sum(c => c.Value);
            var monthCounter = (await _counterDatedDao.GetPersonalCounters(chatId, user.Id, msgDate - monthSpan, msgDate))
                .Sum(c => c.Value);
            var lastMessageDate = (await _counterDatedDao.GetPersonalLastCounter(chatId, user.Id)).Date.Date;

            return $"<b>{user.GetFullName()}</b>\n\nА че это за чел?\n\n" +
                   $"Total count - 0d{totalCounter} <i>words</i>.\n\n" +
                   $"Today count - 0d{todayCounter} <i>words</i>.\n" +
                   $"This week count - 0d{weekCounter} <i>words</i>.\n" +
                   $"This month count - 0d{monthCounter} <i>words</i>.\n\n" +
                   $"Last message was on {lastMessageDate:dd MMMM yyyy}\n";
        }
    }
}