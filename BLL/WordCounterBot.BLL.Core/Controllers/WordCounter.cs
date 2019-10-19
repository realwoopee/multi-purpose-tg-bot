using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.DAL.Contracts;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class WordCounter : IHandler
    {
        private readonly ICounterDao _counterDao;

        public WordCounter(ICounterDao counterDao)
        {
            _counterDao = counterDao;
        }

        public async  Task<bool> Predicate(Update update) =>
            await Task.Run(() => update.Message?.Text != null && !update.Message.Text.StartsWith('/'));

        public async Task HandleUpdate(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var msgId = update.Message.From.Id;
            var wordsCount = WordCounterUtil.CountWords(update.Message.Text);

            if (await _counterDao.CheckCounter(chatId, msgId))
            {
                await _counterDao.IncrementCounter(
                        chatId,
                        msgId,
                        wordsCount);
            }
            else
            {
                await _counterDao.AddCounter(
                        chatId,
                        msgId,
                        wordsCount);
            }
        }
    }
}
