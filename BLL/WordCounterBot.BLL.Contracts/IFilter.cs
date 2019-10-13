using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Contracts
{
    public interface IFilter
    {
        bool Predicate(Update update);
    }
}