using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;

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

            if (_config.UseSocks5)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new HttpToSocks5Proxy(_config.Socks5Host, _config.Socks5Port),
                    UseProxy = true
                };
                var httpClient = new HttpClient(httpClientHandler);

                _botClient = new TelegramBotClient(_config.TelegramToken, httpClient);
            }

            _botClient = new TelegramBotClient(_config.TelegramToken);
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _config.LogLevel;

        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if(!IsEnabled(logLevel)) return;

            if (_config.EventId == 0 || _config.EventId == eventId.Id)
            {
                var message = $"<b>Level</b>: {logLevel.ToString().Escape()}\n" +
                              $"<b>EventId</b>: {eventId.Id.ToString().Escape()}\n" +
                              $"<b>LoggerName</b>: {_name.Escape()}\n" +
                              $"<b>Error Formatted</b>: {formatter(state, exception).Escape()}\n";
                if (exception != null)
                {
                    message += $"<b>Exception message</b>: {exception.Message.Escape()}\n" +
                               $"<b>Stack trace</b>: <code>{exception.StackTrace.Escape()}</code>";
                }
                
                await _botClient.SendTextMessageAsync(_config.UserId, message, ParseMode.Html);
            }
        }
    }
}
