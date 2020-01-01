using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Contracts
{
    public interface IHandler
    {
        Task<bool> IsHandable(Update update);
        Task HandleUpdate(Update update);
    }
}
