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
            EnsurePfxCertExists();
            CreateHostBuilder(args).Build().Run();
        }

        private static void EnsurePfxCertExists()
        {
            var certbotEnabled =
                Environment.GetEnvironmentVariable("certbotEnabled") == "true";

            if (!certbotEnabled)
            {
                return;
            }

            var pfxFilePath = 
                Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path");
            var pemCertPath = Environment.GetEnvironmentVariable("SLLCertPath");
            var pemKeyPath = Environment.GetEnvironmentVariable("CertKeyPath");
            var certPassword =
                Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password");

            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")?.ToLowerInvariant();
            
            if (string.IsNullOrEmpty(pfxFilePath) 
                || string.IsNullOrEmpty(pemCertPath) 
                || string.IsNullOrEmpty(pemKeyPath) 
                || string.IsNullOrEmpty(certPassword)
                || (!string.IsNullOrEmpty(environment) && environment == "development"))
            {
                return;
            }

            var certificateExists = false;

            while (!certificateExists)
            {
                if (File.Exists(pfxFilePath))
                {
                    certificateExists = true;
                    continue;
                }

                if (!File.Exists(pemCertPath) || !File.Exists(pemKeyPath))
                {
                    Thread.Sleep(5000);
                    continue;
                }

                var command = $@"openssl pkcs12 -export -out {pfxFilePath} -inkey {pemKeyPath} -passin 'pass:{certPassword}' -passout 'pass:{certPassword}' -in {pemCertPath}";
                
                var startInfo = new ProcessStartInfo()
                    { FileName = "/bin/bash", Arguments = command, };
                var process = new Process() { StartInfo = startInfo, };
                process.Start();
                process.WaitForExit();
                if (process.ExitCode != 0)
                {
                    throw new Exception("openssl failed");
                }
            }
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
