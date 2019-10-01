using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.DAL.Postgresql;

namespace WordCounterBot.BLL.Core.Controllers
{
    public class WordCounter : IHandler
    {
        private ICounterDao _counterDao;
        private WordCounterUtil _util;

        public WordCounter(ICounterDao dao, WordCounterUtil util)
        {
            _counterDao = dao;
            _util = util;
        }

        public async Task HandleUpdate(Update update)
        {
            var chatId = update.Message.Chat.Id;
            var msgId = update.Message.From.Id;
            var wordsCount = _util.CountWords(update.Message.Text);

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
