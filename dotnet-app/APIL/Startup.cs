using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Newtonsoft.Json;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.BLL.Core;
using WordCounterBot.BLL.Core.Controllers;
using WordCounterBot.Common.Entities;
using WordCounterBot.Common.Logging;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.DAL.Postgresql;

namespace WordCounterBot.APIL.WebApi
{
    public class Startup
    {
        private readonly TelegramBotClient _botClient;
        private readonly AppConfiguration _appConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _appConfig = new AppConfiguration(Configuration);

            if (_appConfig.UseSocks5)
            {
                var httpClientHandler = new HttpClientHandler
                {
                    Proxy = new HttpToSocks5Proxy(_appConfig.Socks5Host, _appConfig.Socks5Port),
                    UseProxy = true
                };
                var httpClient = new HttpClient(httpClientHandler);

                _botClient = new TelegramBotClient(_appConfig.TelegramToken, httpClient);
            }

            _botClient = new TelegramBotClient(_appConfig.TelegramToken);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddSingleton(_appConfig);

            services.AddSingleton(_botClient);

            services.AddScoped<ICounterDao, CounterDao>();
            services.AddScoped<ICounterDatedDao, CounterDatedDao>();
            services.AddScoped<IUserDao, UserDaoPostgreSQL>();

            services.AddTransient<ICommand, GetPersonalStatsCommand>();
            services.AddTransient<ICommand, GetCountersCommand>();
            services.AddTransient<ICommand, GetStatsForCurrentDayCommand>();
            services.AddTransient<ICommand, GetStatsForLastWeekCommand>();
            services.AddTransient<IHandler, CommandExecutor>();

            services.AddTransient<IHandler, SystemMessageHandler>();
            services.AddTransient<IHandler, WordCounter>();
            services.AddTransient<IHandler, UserInfoHandler>();
            services.AddTransient<IHandler, StringReplacer>();

            services.AddScoped<IRouter, UpdateRouter>();

            services.AddScoped<StatusService>();

            services.AddMemoryCache();
            services.AddTransient<MemoryMessageStorage>();
            
            services.AddLogging(builder => builder
                .AddProvider(new TelegramMessengerLoggerProvider(
                    new TelegramMessengerLoggerConfiguration
                    {
                        LogLevel = LogLevel.Warning,
                        TelegramToken = _appConfig.TelegramToken,
                        UserId = _appConfig.UserIdForLogger,
                        UseSocks5 = _appConfig.UseSocks5,
                        Socks5Host = _appConfig.Socks5Host,
                        Socks5Port = _appConfig.Socks5Port
                    }))
                .AddConsole(options => options.LogToStandardErrorThreshold = LogLevel.Trace));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            if (_appConfig.IsSSLCertSelfSigned)
            {
                var certFileInfo = env.ContentRootFileProvider.GetFileInfo(_appConfig.SSLCertPath);
                var sslCert = new InputFileStream(certFileInfo.CreateReadStream());

                _botClient.DeleteWebhookAsync()
                    .ContinueWith(async (t) =>
                        await _botClient.SetWebhookAsync(_appConfig.WebhookUrl.ToString(), sslCert))
                    .ContinueWith((t) =>
                        logger.LogInformation(
                            "Set webhook to {Url}, SSL cert is {Cert}", _appConfig.WebhookUrl,
                            certFileInfo.Name));
            }
            else
            {
                _botClient.DeleteWebhookAsync()
                    .ContinueWith(async (t) =>
                        await _botClient.SetWebhookAsync(_appConfig.WebhookUrl.ToString()))
                    .ContinueWith((t) =>
                        logger.LogInformation(
                            "Set webhook to {Url}", _appConfig.WebhookUrl));
            }

            logger.LogInformation("Configured HTTP pipeline. AppSettings is {Config}", 
                JsonConvert.SerializeObject(_appConfig, Formatting.Indented));
        }
    }
}
