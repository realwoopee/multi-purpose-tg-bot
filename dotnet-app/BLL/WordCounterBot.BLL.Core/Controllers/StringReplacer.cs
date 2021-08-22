using System;
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
        
        public async Task<bool> IsHandable(Update update) =>
            update.Message?.Text?.Length > 0
            && update.Message?.ReplyToMessage?.Text?.Length > 0
            && ReplaceHelper.IsHandable(update.Message.Text);

        public async Task HandleUpdate(Update update)
        {
            var input = update.Message.ReplyToMessage.Text;
            var pattern = update.Message.Text;
            
            try
            {
                var reply = ReplaceHelper.Replace(input, pattern);

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