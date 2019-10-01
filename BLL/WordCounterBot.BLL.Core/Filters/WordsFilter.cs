using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Core.Filters
{
    public class WordsFilter : IFilter
    {
        public bool Predicate(Update update) =>
            update.Message.Text != null;
    }
}
