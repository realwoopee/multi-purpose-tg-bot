using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types;

namespace WordCounterBot.Common.Logging
{
    public class NullLogger : ILogger
    {
        public void Log(object @object) { }

        public void LogData(Update update) { }

        public void LogException(string message) { }
    }
}
