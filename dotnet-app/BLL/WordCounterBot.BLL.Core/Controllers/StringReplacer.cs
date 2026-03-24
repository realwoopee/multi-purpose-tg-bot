using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class StringReplacer : IHandler
    {
        private readonly MessageSender _messageSender;

        public StringReplacer(MessageSender messageSender)
        {
            _messageSender = messageSender;
        }

        public async Task<bool> IsHandleable(Update update, HandleContext context)
        {
            var canPerformReplacement =
                update.Message?.Text?.Length > 0
                && update.GetReplyText()?.Length > 0;
            if (!canPerformReplacement)
                return false;

            var patterns = update.Message.Text.Split('\n', '\r').ToList();
            return ReplaceHelper.IsHandable(patterns);
        }

        public async Task<bool> HandleUpdate(Update update, HandleContext context)
        {
            var input = update.GetReplyText();
            var patterns = update.Message.Text.Split('\n', '\r').ToList();

            try
            {
                var reply = ReplaceHelper.Replace(input, patterns);

                await _messageSender.SendReplyToMessageAsync(
                    update,
                    update.Message.ReplyToMessage.MessageId,
                    reply
                );
                return true;
            }
            catch (RegexParseException)
            {
                await _messageSender.SendReplyAsync(update, "Regex parse error");
                return false;
            }
        }
    }
}