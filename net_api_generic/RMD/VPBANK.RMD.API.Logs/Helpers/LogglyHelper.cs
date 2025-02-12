using Loggly;
using Loggly.Config;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Events;
using VPBANK.RMD.API.Settings;

namespace VPBANK.RMD.API.Logs.Helpers
{
    public static class LogglyHelper
    {
        public static void Config(IConfiguration configuration)
        {
            // Must have Loggly account and setup correct info in appsettings
            if (bool.Parse(configuration["Serilog:UseLoggly"]))
            {
                var logglySettings = new LogglySettings();
                configuration.GetSection("Serilog:Loggly").Bind(logglySettings);
                SetupLogglyConfiguration(logglySettings);
            }

            // log file name
            var logFilename = configuration.GetSection("Serilog:Loggly:ApplicationName");

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .MinimumLevel.Debug()
                .MinimumLevel.Override(nameof(Microsoft), LogEventLevel.Information)
                .MinimumLevel.Override(nameof(System), LogEventLevel.Warning)
                .MinimumLevel.Override(nameof(Microsoft.AspNetCore), LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.Seq(configuration[nameof(AppSettings.ApiUri)].ToString())
                //Serilog.Sinks.File    //Output---- 1. log.txt, 2. log_001.txt, 3. log_002.txt
                .WriteTo.File(path: $"logs/{logFilename.Value}-.log",
                              outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:l}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day,
                              fileSizeLimitBytes: 512000000,
                              rollOnFileSizeLimit: true,
                              retainedFileCountLimit: null)
                .CreateLogger();
        }

        #region Configure Loggly

        /// <summary>
        /// Configure Loggly provider
        /// </summary>
        /// <param name="logglySettings"></param>
        private static void SetupLogglyConfiguration(LogglySettings logglySettings)
        {
            //Configure Loggly
            var config = LogglyConfig.Instance;
            config.CustomerToken = logglySettings.CustomerToken;
            config.ApplicationName = logglySettings.ApplicationName;
            config.Transport = new TransportConfiguration()
            {
                EndpointHostname = logglySettings.EndpointHostname,
                EndpointPort = logglySettings.EndpointPort,
                LogTransport = logglySettings.LogTransport
            };
            config.ThrowExceptions = logglySettings.ThrowExceptions;

            //Define Tags sent to Loggly
            config.TagConfig.Tags.AddRange(new ITag[]{
                new ApplicationNameTag {Formatter = "Application-{0}"},
                new HostnameTag { Formatter = "Host-{0}" }
            });
        }

        public class LogglySettings
        {
            public string ApplicationName { get; set; }
            public string Account { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public int EndpointPort { get; set; }
            public bool IsEnabled { get; set; }
            public bool ThrowExceptions { get; set; }
            public LogTransport LogTransport { get; set; }
            public string EndpointHostname { get; set; }
            public string CustomerToken { get; set; }
        }

        #endregion
    }
}
