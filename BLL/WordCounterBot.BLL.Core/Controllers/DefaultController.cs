using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class DefaultController : IHandler
    {
        private TelegramBotClient _client;
        public DefaultController(TelegramBotClient client)
        {
            _client = client;
        }

        public async Task HandleUpdate(Update update)
        {
            await _client.SendTextMessageAsync(
                chatId: update.Message.Chat.Id, 
                text: "Принял", 
                replyToMessageId: update.Message.MessageId);
            return;
        }
    }
}
