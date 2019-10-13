using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Filters
{
    public class CommandFilter : IFilter
    {
        public bool Predicate(Update update) => 
            update.Message.Text != null && update.Message.Text.StartsWith('/');
    }
}
