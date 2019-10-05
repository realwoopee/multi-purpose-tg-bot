using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace WordCounterBot.Common.Entities
{
    public class AppConfiguration
    {
        
        public string DbConnectionString { get; set; }

        public string TelegramToken { get; set; }

        public string WebhookUrl { get; set; }

        public bool Socks5Enabled { get; set; }

        public string Socks5Host { get; set; }

        public int Socks5Port { get; set; }

        public AppConfiguration(IConfiguration configuration) :
            this(configuration.GetValue<string>("DB_CONNECTION_STRING"),
                 configuration.GetValue<string>("TOKEN"),
                 configuration.GetValue<string>("WEBHOOK_URL"),
                 configuration.GetValue<bool>("SOCKS5_ENABLED"),
                 configuration.GetValue<string>("SOCKS5_HOST"),
                 configuration.GetValue<int>("SOCKS5_PORT"))
        { }

        public AppConfiguration(string dbConnectionString, string telegramToken, string webhookUrl, bool socks5Enabled, string socks5Host = null, int? socks5Port = null)
        {
            DbConnectionString = dbConnectionString ?? throw new ArgumentNullException(nameof(dbConnectionString));
            TelegramToken = telegramToken ?? throw new ArgumentNullException(nameof(telegramToken));
            WebhookUrl = webhookUrl ?? throw new ArgumentNullException(nameof(webhookUrl));
            Socks5Enabled = socks5Enabled;

            if (Socks5Enabled)
            {
                Socks5Host = socks5Host ?? throw new ArgumentNullException(nameof(socks5Host));
                Socks5Port = socks5Port.HasValue ? socks5Port.Value : throw new ArgumentNullException(nameof(socks5Port));
            }
        }
    }
}
