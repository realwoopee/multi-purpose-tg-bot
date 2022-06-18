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
        private readonly MemoryMessageStorage _messageStorage;

        public WordCounter(ICounterDao counterDao, ICounterDatedDao counterDatedDao, MemoryMessageStorage messageStorage)
        {
            _counterDao = counterDao;
            _counterDatedDao = counterDatedDao;
            _messageStorage = messageStorage;
        }

        public async  Task<bool> IsHandleable(Update update) =>
            await Task.Run(() =>
            {
                var body = (update.Message ?? update.EditedMessage);
                return body?.ForwardFrom == null
                    && body?.ForwardFromChat == null
                    && (body?.Text != null
                        || body?.Caption != null);
            });

        public async Task<bool> HandleUpdate(Update update)
        {
            var body = (update.Message ?? update.EditedMessage);
            var chatId = body.Chat.Id;
            var userId = body.From.Id;
            var date = body.Date.Date;
            var text = body.Text ?? body.Caption;
            int wordsCount;
            
            if (update.Message != null)
            {
                wordsCount = WordCounterUtil.CountWords(text);
            }
            else // update.EditedMessage != null
            {
                var cachedText = _messageStorage.TryGetText(body.MessageId);
                if (cachedText == null)
                {
                    return true;
                }

                wordsCount = -1 * WordCounterUtil.CountWords(cachedText);
            }
            
            _messageStorage.AddOrUpdate(body.MessageId, text);
            
            await _counterDao.UpdateElseCreateCounter(
                chatId,
                userId,
                wordsCount);

            await _counterDatedDao.UpdateElseCreateCounter(
                chatId,
                userId,
                date,
                wordsCount);

            return true;
        }
    }
}
