using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace WordCounterBot.BLL.Core.Filters
{
    public class CommandFilter : IFilter
    {
        public bool Predicate(Update update) => 
            update.Message.Text != null && update.Message.Text.StartsWith('/');
    }
}
