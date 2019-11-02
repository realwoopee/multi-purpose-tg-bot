using Telegram.Bot.Types;

namespace WordCounterBot.Common.Logging
{
    public interface ILogger
    {
        public void Log(object @object);
        void LogException(string message);
        void LogData(Update update);
    }
}