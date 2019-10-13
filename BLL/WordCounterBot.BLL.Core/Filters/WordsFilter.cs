using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Filters
{
    public class WordsFilter : IFilter
    {
        public bool Predicate(Update update) =>
            update.Message.Text != null;
    }
}
