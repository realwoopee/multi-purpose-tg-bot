using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class StringReplacer : IHandler
    {
        private readonly TelegramBotClient _client;

        public StringReplacer(TelegramBotClient client)
        {
            _client = client;
        }
        
        public async Task<bool> IsHandable(Update update)
        {
            var a = update.Message?.Text?.Length > 0
                    && (update.Message?.ReplyToMessage?.Text?.Length > 0
                        || update.Message?.ReplyToMessage?.Caption?.Length > 0);
            if (!a) return false;
            
            var patterns = update.Message.Text.Split('\n', '\r').ToList();
            return ReplaceHelper.IsHandable(patterns);
        }

        public async Task HandleUpdate(Update update)
        {
            var input = update.Message.ReplyToMessage.Text ?? update.Message.ReplyToMessage.Caption;
            var patterns = update.Message.Text.Split('\n', '\r').ToList();
            
            try
            {
                var reply = ReplaceHelper.Replace(input, patterns);

                await _client.SendTextMessageAsync(update.Message.Chat.Id, 
                    reply, 
                    replyToMessageId: update.Message.ReplyToMessage.MessageId);
            }
            catch (RegexParseException e)
            {
                await _client.SendTextMessageAsync(update.Message.Chat.Id,
                    "Regex parse error",
                    replyToMessageId: update.Message.MessageId);
            }
        }
    }
}