using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;

namespace WordCounterBot.BLL.Contracts
{
    public interface IHandler
    {
        Task<bool> IsHandleable(Update update, HandleContext context);
        Task<bool> HandleUpdate(Update update, HandleContext context);
    }
}
