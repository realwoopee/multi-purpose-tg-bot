using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WordCounterBot.APIL.WebApi.Setup;
using WordCounterBot.Common.Entities;

namespace WordCounterBot.APIL.WebApi;

public class Startup
{
    public IConfiguration Configuration { get; }
    private AppConfiguration _appConfig;

    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        _appConfig = new AppConfiguration(Configuration);

        services.AddControllers().AddNewtonsoftJson();
        services.AddSingleton(_appConfig);

        services.AddTelegramBotClient(_appConfig);
        services.AddDataAccessServices();
        services.AddBotServices();
        services.AddTelegramLogging(_appConfig);

        if (_appConfig.UsePolling)
            services.AddHostedService<PollingService>();

        services.Configure<ForwardedHeadersOptions>(options =>
        {
            options.ForwardedHeaders =
                ForwardedHeaders.XForwardedFor
                | ForwardedHeaders.XForwardedProto
                | ForwardedHeaders.XForwardedHost;
            options.KnownProxies.Clear();
            options.KnownNetworks.Clear();
        });
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ILogger<Startup> logger)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseForwardedHeaders();
        }

        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => endpoints.MapControllers());

        if (!_appConfig.UsePolling)
            app.UseTelegramWebhook(env, logger);
    }
}