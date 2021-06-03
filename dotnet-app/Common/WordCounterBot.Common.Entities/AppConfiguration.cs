using Microsoft.Extensions.Configuration;
using System;
using System.Globalization;

namespace WordCounterBot.Common.Entities
{
    public class AppConfiguration
    {
        public string DbConnectionString { get; }

        public string TelegramToken { get; }

        public Uri WebhookUrl { get; }

        public string SSLCertPath { get; }

        public bool UseSocks5 { get; }

        public string Socks5Host { get; }

        public int Socks5Port { get; }

        public int UserIdForLogger { get; }

        public AppConfiguration(IConfiguration configuration)
        {
            if (configuration == null) 
                throw new ArgumentNullException(nameof(configuration));
            
            DbConnectionString = configuration["DbConnectionString"];
            TelegramToken = configuration["TgToken"];
            WebhookUrl = new Uri(configuration["WebhookUrl"]);
            SSLCertPath = configuration["SSLCertPath"];
            UseSocks5 = configuration.GetValue<bool>("UseSocks5");
            Socks5Host = configuration["Socks5Host"];
            Socks5Port = configuration.GetValue<int>("Socks5Port");
            UserIdForLogger = configuration.GetValue<int>("UserToLog");
        }
    }
}
