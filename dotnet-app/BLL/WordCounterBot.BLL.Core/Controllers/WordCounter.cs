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
        private readonly ICounterDatedDao _counterDatedDao;

        public WordCounter(ICounterDao counterDao, ICounterDatedDao counterDatedDao)
        {
            _counterDao = counterDao;
            _counterDatedDao = counterDatedDao;
        }

        public async  Task<bool> IsHandable(Update update) =>
            await Task.Run(() =>
                update.Message?.ForwardFrom == null && update.Message?.ForwardFromChat == null 
                && update.Message?.Text != null && !update.Message.Text.StartsWith('/')
                || update.Message?.Caption != null);

        public async Task HandleUpdate(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var userId = update.Message.From.Id;
            var text = update.Message.Text ?? update.Message.Caption;
            var wordsCount = WordCounterUtil.CountWords(text);
            var currDate = update.Message.Date.Date;

            await _counterDao.UpdateElseCreateCounter(
                        chatId,
                        userId,
                        wordsCount);

            await _counterDatedDao.UpdateElseCreateCounter(
                    chatId,
                    userId,
                    currDate,
                    wordsCount);
        }
    }
}
