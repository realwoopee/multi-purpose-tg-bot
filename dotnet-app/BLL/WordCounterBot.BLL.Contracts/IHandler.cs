using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Contracts
{
    public interface IHandler
    {
        Task<bool> IsHandleable(Update update);
        Task<bool> HandleUpdate(Update update);
    }
}
