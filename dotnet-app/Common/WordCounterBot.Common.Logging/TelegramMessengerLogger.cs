using System;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WordCounterBot.Common.Logging
{
    public class TelegramMessengerLogger : ILogger
    {
        private readonly string _name;
        private readonly TelegramMessengerLoggerConfiguration _config;
        private readonly TelegramBotClient _botClient;

        public TelegramMessengerLogger(string name, TelegramMessengerLoggerConfiguration config)
        {
            _name = name;
            _config = config;

            HttpToSocks5Proxy proxy = null;
            if (_config.UseSocks5)
            {
                proxy = new HttpToSocks5Proxy(_config.Socks5Host, _config.Socks5Port);
            }

            _botClient = new TelegramBotClient(
                _config.TelegramToken,
                proxy);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _config.LogLevel;

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel)) return;

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                var message = $"<b>Level</b>: {logLevel}\n" +
                              $"<b>EventId</b>: {eventId.Id}\n" +
                              $"<b>LoggerName</b>: {_name}\n" +
                              $"<b>Error Formatted</b>: {formatter(state, exception)}\n";
                if (exception != null)
                {
                    message += $"<b>Exception message</b>: {exception.Message}\n" +
                               $"<b>Stack trace</b>: <code>{exception.StackTrace}</code>";
                }
                
                await _botClient.SendTextMessageAsync(_config.UserId, message, ParseMode.Html);
            }
        }
    }
}
