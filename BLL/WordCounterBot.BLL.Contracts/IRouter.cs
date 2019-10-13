using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Contracts
{
    public interface IRouter
    {
        Task Route(Update update);
    }
}
