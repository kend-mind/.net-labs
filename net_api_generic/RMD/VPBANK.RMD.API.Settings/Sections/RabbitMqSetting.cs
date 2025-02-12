using Microsoft.Extensions.Configuration;
using static VPBANK.RMD.API.Settings.AppSettings;

namespace VPBANK.RMD.API.Settings
{
    public static class RabbitMqSetting
    {
        public static RabbitMqConf GetRabbitMqSetting(IConfiguration configuration)
        {
            return new RabbitMqSection
            {
                Password = configuration["RabbitMq:Password"],
                UserName = configuration["RabbitMq:UserName"],
                HostName = configuration["RabbitMq:HostName"],
                VirtualHostUrl = configuration["RabbitMq:VirtualHostUrl"],
                Port = int.Parse(configuration["RabbitMq:Port"]),
                APIUrl = configuration["RabbitMq:APIUrl"],
                APIKey = configuration["RabbitMq:APIKey"],
                APISecretKey = configuration["RabbitMq:APISecretKey"],
                NotiExchange = configuration["RabbitMq:NotiExchange"],
                NotiQueue = configuration["RabbitMq:NotiQueue"],
                RouterKey = configuration["RabbitMq:RouterKey"]
            };
        }
    }
}
