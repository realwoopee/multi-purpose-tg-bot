using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MihaZupan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using WordCounterBot.BLL.Contracts;
using WordCounterBot.BLL.Core;
using WordCounterBot.BLL.Core.Controllers;
using WordCounterBot.BLL.Core.Filters;
using WordCounterBot.BLL.Common;
using WordCounterBot.DAL.Contracts;
using WordCounterBot.DAL.Postgresql;
using WordCounterBot.Common.Logging;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Npgsql;
using WordCounterBot.Common.Entities;

namespace WordCounterBot
{
    public class Startup
    {
        private readonly TelegramBotClient _botClient;
        private readonly AppConfiguration _appConfig;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _appConfig = new AppConfiguration(configuration);

            IWebProxy _proxy = null;

            if (_appConfig.Socks5Enabled)
            {
                _proxy = new HttpToSocks5Proxy(_appConfig.Socks5Host, _appConfig.Socks5Port);
            }

            _botClient = new TelegramBotClient(
                _appConfig.TelegramToken,
                _proxy);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddTransient<ILogger, NullLogger>();
            services.AddTransient<WordCounterUtil>();
            services.AddSingleton(_appConfig);

            services.AddSingleton(_botClient);

            services.AddTransient<ICounterDao, CounterDao>();

            services.AddTransient<CommandExecutor>();
            services.AddTransient<WordCounter>();
            services.AddTransient<DefaultController>();

            services.AddTransient<CommandFilter>();
            services.AddTransient<WordsFilter>();

            services.AddTransient<IRouter, UpdateRouter>(
                services => new UpdateRouter(services.GetRequiredService<ILogger>())
                    {
                        DefaultHandler = services.GetRequiredService<DefaultController>(),
                        Handlers = new List<(IFilter, IHandler)>()
                            {
                                (services.GetService<CommandFilter>(), services.GetService<CommandExecutor>()),
                                (services.GetService<WordsFilter>(), services.GetService<WordCounter>())
                            }
                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            _botClient.DeleteWebhookAsync()
                .ContinueWith(async (t) => await _botClient.SetWebhookAsync(Configuration["WEBHOOK_URL"]));
        }
    }
}
