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
using WordCounterBot.Common.Entities;
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
            services.AddSingleton(_appConfig);

            services.AddSingleton(_botClient);
            services.AddScoped<UserUpdater>();

            services.AddScoped<ICounterDao, CounterDao>();
            services.AddScoped<IUserDao, UserDao>();

            services.AddTransient<ICommand, GetCountersCommand>();
            services.AddTransient<IHandler, CommandExecutor>();

            services.AddTransient<IHandler, SystemMessageHandler>();
            services.AddTransient<IHandler, WordCounter>();
            services.AddTransient<DefaultHandler>();

            services.AddScoped<IRouter, UpdateRouter>();
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
