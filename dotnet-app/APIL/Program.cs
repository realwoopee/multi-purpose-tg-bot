using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace WordCounterBot.APIL.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel()
                        .UseStartup<Startup>();
                });
    }
}
