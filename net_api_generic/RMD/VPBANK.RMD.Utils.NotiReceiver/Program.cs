using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Serilog;
using RabbitMQ.Client;
using VPBANK.RMD.Utils.NotiReceiver.Extentions;
using VPBANK.RMD.Utils.Notification.Receiver;

namespace VPBANK.RMD.Utils.NotiReceiver
{
    class Program
    {
        public static IConfigurationRoot configuration;

        static int Main(string[] args)
        {
            try
            {
                // Start!
                Console.WriteLine("Hello World! Started rabbitmq client.");
                MainAsync(args).Wait();
                return 0;
            }
            catch
            {
                return 1;
            }
        }

        static async Task MainAsync(string[] args)
        {
            // Create service collection
            Log.Information("Creating service collection");
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Create service provider
            Log.Information("Building service provider");
            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            //RabbitMq use
            IApplicationBuilder app = new ApplicationBuilder(serviceProvider);
            app.UseRabbitListener();

            // Print connection string to demonstrate configuration object is populated
            Console.WriteLine(configuration.GetConnectionString("DataConnection"));

            try
            {
                Log.Information("Starting service");
                await serviceProvider.GetService<App>().RunAsync();
                Log.Information("Ending service");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error running service");
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        static void ConfigureServices(IServiceCollection serviceCollection)
        {
            // Add logging
            serviceCollection.AddSingleton(LoggerFactory.Create(builder =>
            {
                builder.AddSerilog(dispose: true);
            }));

            serviceCollection.AddLogging();

            // Build configuration
            configuration = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                //.AddJsonFile("appsettings.Production.json", optional: true, reloadOnChange: true)
                //.AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .Build();

            // Add access to generic IConfigurationRoot
            serviceCollection.AddSingleton<IConfigurationRoot>(configuration);

            // RabbitMq conf
            serviceCollection.AddTransient<IRabbitMQPersistentConnection, RabbitMQPersistentConnection>();
            serviceCollection.AddSingleton(cf =>
            {
                var factory = new ConnectionFactory()
                {
                    HostName = configuration["RabbitMq:HostName"],
                    UserName = configuration["RabbitMq:UserName"],
                    Password = configuration["RabbitMq:Password"],
                    Port = int.Parse(configuration["RabbitMq:Port"]),
                    VirtualHost = configuration["RabbitMq:VirtualHostUrl"]
                };
                return new RabbitMQPersistentConnection(factory, serviceCollection, configuration);
            });

            // Add app
            serviceCollection.AddOptions();
            serviceCollection.AddTransient<App>();
        }
    }
}
