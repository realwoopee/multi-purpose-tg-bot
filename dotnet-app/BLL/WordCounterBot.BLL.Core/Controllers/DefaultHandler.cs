using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class DefaultHandler : IHandler
    {
        public async Task<bool> IsHandable(Update update) => 
            await Task.Run(() => true);

        public async Task HandleUpdate(Update update) => 
            await Task.CompletedTask;
    }
}
