using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class DefaultHandler : IHandler
    {
        public async Task<bool> IsHandleable(Update update, HandleContext context) =>
            await Task.FromResult(false);

        public async Task<bool> HandleUpdate(Update update, HandleContext context) =>
            await Task.FromResult(true);
    }
}
