using System.Collections.Generic;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MihaZupan;
using Telegram.Bot;
using Telegram.Bot.Types.InputFiles;
using WordCounterBot.BLL.Common;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.BLL.Core;
using WordCounterBot.BLL.Core.Controllers;
using WordCounterBot.BLL.Core.Filters;
using WordCounterBot.Common.Entities;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.DAL.Postgresql;

namespace WordCounterBot.APIL.WebApi
{
    public class Startup
    {
        private TelegramBotClient _botClient;
        private AppConfiguration _appConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _appConfig = new AppConfiguration(Configuration);

            IWebProxy proxy = null;

            if (_appConfig.UseSocks5)
            {
                proxy = new HttpToSocks5Proxy(_appConfig.Socks5Host, _appConfig.Socks5Port);
            }

            _botClient = new TelegramBotClient(
                _appConfig.TelegramToken,
                proxy);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();
            services.AddTransient<WordCounterUtil>();
            services.AddSingleton(_appConfig);

            services.AddSingleton(_botClient);

            services.AddTransient<ICounterDao, CounterDao>();

            services.AddTransient<SystemMessageHandler>();
            services.AddTransient<CommandExecutor>();
            services.AddTransient<WordCounter>();
            services.AddTransient<DefaultHandler>();

            services.AddTransient<SystemMessageFilter>();
            services.AddTransient<CommandFilter>();
            services.AddTransient<WordsFilter>();

            services.AddTransient<IRouter, UpdateRouter>(
                services => new UpdateRouter(services.GetRequiredService<ILogger<UpdateRouter>>())
                    {
                        DefaultHandler = services.GetRequiredService<DefaultHandler>(),
                        Handlers = new List<(IFilter, IHandler)>()
                            {
                                (services.GetService<SystemMessageFilter>(), services.GetService<SystemMessageHandler>()),
                                (services.GetService<CommandFilter>(), services.GetService<CommandExecutor>()),
                                (services.GetService<WordsFilter>(), services.GetService<WordCounter>())
                            }
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            InputFileStream sslCert = null;

            if (!string.IsNullOrEmpty(_appConfig.SSLCertPath))
            {
                sslCert = new InputFileStream(env.ContentRootFileProvider.GetFileInfo(_appConfig.SSLCertPath).CreateReadStream());
            }

            _botClient.DeleteWebhookAsync()
                .ContinueWith(async (t) => await _botClient.SetWebhookAsync(_appConfig.WebhookUrl, sslCert))
                .ContinueWith((t) => logger.LogInformation($"Set webhook to {_appConfig.WebhookUrl}, SSL cert is {sslCert?.ToString() ?? "null"}"));

            logger.LogInformation($"Configured HTTP pipeline. AppSettings is {_appConfig}");
        }
    }
}
