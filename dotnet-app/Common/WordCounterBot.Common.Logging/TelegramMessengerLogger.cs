using System;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Common.Helpers;
using WordCounterBot.BLL.Common.Services;

namespace WordCounterBot.Common.Logging
{
    public class TelegramMessengerLogger : ILogger
    {
        private readonly string _name;
        private readonly TelegramMessengerLoggerConfiguration _config;
        private readonly MessageSender _sender;

        public TelegramMessengerLogger(string name, TelegramMessengerLoggerConfiguration config)
        {
            _name = name;
            _config = config;

            var botClient = InitTelegramClient();

            _sender = new MessageSender(botClient);
        }

        private TelegramBotClient InitTelegramClient()
        {
            TelegramBotClient botClient;

            if (_config.UseSocks5)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new HttpToSocks5Proxy(_config.Socks5Host, _config.Socks5Port),
                    UseProxy = true
                };
                var httpClient = new HttpClient(httpClientHandler);

                botClient = new TelegramBotClient(_config.TelegramToken, httpClient);
            }
            else
            {
                botClient = new TelegramBotClient(_config.TelegramToken);
            }

            return botClient;
        }

        public IDisposable BeginScope<TState>(TState state) => null;

        public bool IsEnabled(LogLevel logLevel) => logLevel >= _config.LogLevel;

        public async void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter
        )
        {
            if (!IsEnabled(logLevel))
                return;

            if (_config.EventId != 0 && _config.EventId != eventId.Id) return;
            
            var message =
                $"{"Level".HtmlBold()}: {logLevel.ToString().HtmlEscape()}\n"
                + $"{"EventId".HtmlBold()}: {eventId.Id.ToString().HtmlEscape()}\n"
                + $"{"LoggerName".HtmlBold()}: {_name.HtmlEscape()}\n"
                + $"{"Error Formatted".HtmlBold()}: {formatter(state, exception).HtmlEscape()}\n";
            if (exception != null)
            {
                message +=
                    $"{"Exception message".HtmlBold()}: {exception.Message.HtmlEscape()}\n"
                    + $"{"Stack trace".HtmlBold()}: {exception.StackTrace.HtmlEscape().HtmlCode()}";
            }

            await _sender.SendHtmlToChatAsync(_config.UserId, message);
        }
    }
}
