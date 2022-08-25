using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace WordCounterBot.Common.Logging
{
    public class TelegramMessengerLoggerProvider : ILoggerProvider
    {
        private readonly TelegramMessengerLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, TelegramMessengerLogger> _loggers =
            new ConcurrentDictionary<string, TelegramMessengerLogger>();

        public TelegramMessengerLoggerProvider(TelegramMessengerLoggerConfiguration config)
        {
            _config = config;
        }

        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(
                categoryName,
                name => new TelegramMessengerLogger(name, _config)
            );
        }

        public void Dispose()
        {
            _loggers.Clear();
        }
    }
}
