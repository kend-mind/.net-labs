using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using VPBANK.RMD.Utils.Notification.Receiver;

namespace VPBANK.RMD.Utils.NotiReceiver.Extentions
{
    public static class RabbitMqBuilderExtentions
    {
        public static RabbitMQPersistentConnection Listener { get; set; }

        public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app)
        {
            Listener = app.ApplicationServices.GetService<RabbitMQPersistentConnection>();
            Listener.CreateConsumerChannel();

            // TODO:
            //var life = app.ApplicationServices.GetService<IHostApplicationLifetime>();
            //life.ApplicationStarted.Register(OnStarted);

            ////press Ctrl+C to reproduce if your app runs in Kestrel as a console app
            //life.ApplicationStopping.Register(OnStopping);
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
}
