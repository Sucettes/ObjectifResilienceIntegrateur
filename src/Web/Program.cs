using System;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Gwenael.Web.App_Startup;

namespace Gwenael.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("****************************************");
            Console.WriteLine($"dotnet.exe process id: {Process.GetCurrentProcess().Id}");
            Console.WriteLine($"ASPNETCORE_ENVIRONMENT is {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
            Console.WriteLine("****************************************");
            BuildWebHost(args).Run();
        }

        public static IHost BuildWebHost(string[] args) => CreateWebHostBuilder(args).Build();

        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                    webBuilder
                        .ConfigureAppConfiguration(config => config.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true))
                        .ConfigureLogging((ctx, logging) =>
                        {
                            logging.AddFilter<ConsoleLoggerProvider>(level =>
                                level == LogLevel.None);
                            Logging.Configure(logging, ctx.Configuration);
                        })
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                );
    }
}