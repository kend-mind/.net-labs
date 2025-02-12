using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using VPBANK.RMD.API.Settings;
using Microsoft.Extensions.ObjectPool;
using RabbitMQ.Client;
using VPBANK.RMD.Utils.Notification.Publisher;
using VPBANK.RMD.Utils.Notification.Receiver;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace VPBANK.RMD.API.Common.Features
{
    public static class RabbitMQConfigurator
    {
        /// <summary>
        /// RabbitMq_Conf
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddConfRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            // add conf
            var rabbitConfig = configuration.GetSection("RabbitMq");
            services.Configure<RabbitMqConf>(rabbitConfig);
            return services;
        }

        /// <summary>
        /// RabbitMq_Publicer
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddPublicerRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            // Publicer
            services.AddSingleton<ObjectPoolProvider, DefaultObjectPoolProvider>();
            services.AddSingleton<IPooledObjectPolicy<IModel>, RabbitMqPooledObjectPolicy>();
            services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
            return services;
        }

        /// <summary>
        /// RabbitMq_Receiver
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddReceiverRabbitMQ(this IServiceCollection services, IConfiguration configuration)
        {
            // Receiver
            services.AddSingleton<RabbitMQPersistentConnection>(sp =>
            {
                var logger = sp.GetRequiredService<ILogger<RabbitMQPersistentConnection>>();
                var confRabbitMq = RabbitMqSetting.GetRabbitMqSetting(configuration);
                var factory = new ConnectionFactory()
                {
                    HostName = confRabbitMq.HostName,
                    UserName = confRabbitMq.UserName,
                    Password = confRabbitMq.Password,
                    Port = confRabbitMq.Port,
                    VirtualHost = confRabbitMq.VirtualHostUrl
                };
                return new RabbitMQPersistentConnection(factory, services, configuration);
            });

            return services;
        }
    }
}

public static class RabbitListenerExtentions
{
    public static RabbitMQPersistentConnection Listener { get; set; }

    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
    {
        Listener = app.ApplicationServices.GetService<RabbitMQPersistentConnection>();
        var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
        life.ApplicationStarted.Register(OnStarted);

        // Press Ctrl+C to reproduce if your app runs in Kestrel as a console app
        life.ApplicationStopping.Register(OnStopping);
        return app;
    }

    private static void OnStarted()
    {
        Listener.CreateConsumerChannel();
    }

    private static void OnStopping()
    {
        Listener.Disconnect();
    }
}
