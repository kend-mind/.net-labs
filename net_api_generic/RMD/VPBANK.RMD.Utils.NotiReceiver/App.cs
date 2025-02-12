using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using Serilog;

namespace VPBANK.RMD.Utils.NotiReceiver
{
    public class App
    {
        private readonly IConfigurationRoot _config;
        private readonly ILogger<App> _logger;

        public App(IConfigurationRoot config, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<App>();
            _config = config;
        }

        public void Run()
        {
            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.File(_config.GetValue<string>("Runtime:LogOutputDirectory"))
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();
            _logger.LogInformation("Hello, from Serilog logging");
        }

        public async Task RunAsync()
        {
            //List<string> emailAddresses = _config.GetSection("EmailAddresses").Get<List<string>>();
            //foreach (string emailAddress in emailAddresses)
            //{
            //    _logger.LogInformation("Email address: {@EmailAddress}", emailAddress);
            //}

            // Initialize serilog logger
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .WriteTo.File(_config.GetValue<string>("Runtime:LogOutputDirectory"))
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .CreateLogger();
            _logger.LogInformation("Hello, from Serilog logging");
        }
    }
}
