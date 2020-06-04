using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace WordCounterBot.Common.Logging
{
    public class TelegramMessengerLoggerConfiguration
    {
        public LogLevel LogLevel { get; set; } = LogLevel.Warning;
        public int EventId { get; set; } = 0;

        public int UserId { get; set; }
        public string TelegramToken { get; set; }
        public bool UseSocks5 { get; set; }
        public string Socks5Host { get; set; }
        public int Socks5Port { get; set; }
    }
}
