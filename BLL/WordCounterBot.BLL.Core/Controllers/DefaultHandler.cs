using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class DefaultHandler : IHandler
    {
        private TelegramBotClient _client;
        public DefaultHandler(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task HandleUpdate(Update update)
        {
            await Task.CompletedTask;
        }
    }
}
