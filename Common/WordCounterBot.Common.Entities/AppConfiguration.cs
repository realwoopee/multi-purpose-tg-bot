using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WordCounterBot.Common.Entities
{
    public class AppConfiguration
    {
        
        public string DbConnectionString { get; set; }

        public string TelegramToken { get; set; }

        public string WebhookUrl { get; set; }

        public string SSLCertPath { get; set; }

        public bool UseSocks5 { get; set; }

        public string Socks5Host { get; set; }

        public int Socks5Port { get; set; }

        public AppConfiguration(IConfiguration configuration)
        {
            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var appConf = configuration.GetSection("Configuration");

            DbConnectionString = appConf.GetValue<string>("DbConnectionString");
            TelegramToken = appConf.GetValue<string>("TgToken");
            WebhookUrl = appConf.GetValue<string>("WebhookUrl");
            SSLCertPath = appConf.GetValue<string>("SSLCertPath");
            UseSocks5 = appConf.GetValue<bool>("UseSocks5");
            Socks5Host = appConf.GetValue<string>("Socks5Host");
            Socks5Port = appConf.GetValue<int>("Socks5Port");

        }

        public AppConfiguration(string dbConnectionString, string telegramToken, string webhookUrl, string pathToSSLCert, bool socks5Enabled, string socks5Host = null, int? socks5Port = null)
        {
            DbConnectionString = dbConnectionString ?? throw new ArgumentNullException(nameof(dbConnectionString));
            TelegramToken = telegramToken ?? throw new ArgumentNullException(nameof(telegramToken));
            SSLCertPath = pathToSSLCert ?? throw new ArgumentNullException(nameof(telegramToken));
            WebhookUrl = webhookUrl ?? throw new ArgumentNullException(nameof(webhookUrl));
            UseSocks5 = socks5Enabled;

            if (UseSocks5)
            {
                Socks5Host = socks5Host ?? throw new ArgumentNullException(nameof(socks5Host));
                Socks5Port = socks5Port ?? throw new ArgumentNullException(nameof(socks5Port));
            }
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
