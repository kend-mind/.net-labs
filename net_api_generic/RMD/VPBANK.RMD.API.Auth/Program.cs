using System;
using System.IO;
using NLog.Web;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using VPBANK.RMD.API.Configurations;
using VPBANK.RMD.API.Logs.Helpers;

namespace VPBANK.RMD.API.Auth
{
    public class Program
    {
        private static string _envName;
        private static readonly string _namespace = typeof(Program).Namespace;

        public static int Main(string[] args)
        {
            try
            {
                var configuration = ConfigurationHelper.GetConfiguration(_envName, Directory.GetCurrentDirectory());
                LogglyHelper.Config(configuration);
                NLogBuilder.ConfigureNLog("nlog.config");

                Log.Information($"Configuring web host ({_namespace})");
                CreateHostBuilder(args).Run();
                Log.Information($"Starting web host ({_namespace})");
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Program terminated unexpectedly ({ApplicationContext})!", _namespace);
                return 1;
            }
            finally
            {
                Log.Information("Application started. Press [Ctrl+C] to shut down.");
                Log.CloseAndFlush();
            }
        }

        public static IHost CreateHostBuilder(string[] args) => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseIISIntegration();
            })
            .ConfigureLogging((hostingContext, config) =>
            {
                config.ClearProviders();
                _envName = hostingContext.HostingEnvironment.EnvironmentName;
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .UseNLog()
            .UseSerilog()
            .Build();
    }
}
