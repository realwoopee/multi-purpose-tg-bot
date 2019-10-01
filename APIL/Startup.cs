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

namespace WordCounterBot
{
    public class Startup
    {
        private TelegramBotClient _botClient;
        private IWebProxy _proxy;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            _proxy = new HttpToSocks5Proxy(Configuration["SOCKS5_HOST"], Configuration.GetValue<int>("SOCKS5_PORT"));

            _botClient = Configuration.GetValue<bool>("SOCKS5_ENABLED") ? new TelegramBotClient(
                Configuration["TOKEN"],
                _proxy)
                : new TelegramBotClient(
                Configuration["TOKEN"]);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson();

            services.AddTransient<ILogger, NullLogger>();
            services.AddTransient<WordCounterUtil>();

            services.AddTransient<ICounterDao, CounterDao>();

            //services.AddTransient<IController, CommandExecutor>();
            //services.AddTransient<IController, DefaultController>();
            //services.AddTransient<IController, WordCounter>();

            services.AddTransient<CommandExecutor>();
            services.AddTransient<WordCounter>();
            services.AddTransient<DefaultController>();

            //services.AddTransient<IFilter, CommandFilter>();
            //services.AddTransient<IFilter, WordsFilter>();

            services.AddTransient<CommandFilter>();
            services.AddTransient<WordsFilter>();

            var serviceBuilder = services.BuildServiceProvider();

            var routerConfig = new RouterConfig(serviceBuilder)
            {
                DefaultControllerPreset = typeof(DefaultController),
                ControllerPresets = new List<RouterConfigPair>()
                {
                    new RouterConfigPair { Filter = typeof(WordsFilter), Controller = typeof(WordCounter) },
                    new RouterConfigPair { Filter = typeof(CommandFilter), Controller = typeof(CommandExecutor) },
                }
            };

            services.AddSingleton(routerConfig);
            services.AddTransient<IRouter, UpdateRouter>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
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
