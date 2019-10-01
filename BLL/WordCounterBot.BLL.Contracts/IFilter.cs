using System;
using System.Linq;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Core
{
    public interface IFilter
    {
        public bool Predicate(Update update);
    }
}