using Microsoft.Extensions.ObjectPool;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using VPBANK.RMD.API.Settings;

namespace VPBANK.RMD.Utils.Notification.Publisher
{
    public class RabbitMqPooledObjectPolicy : IPooledObjectPolicy<IModel>
    {
        private readonly RabbitMqConf _options;
        private readonly IConnection _connection;

        public RabbitMqPooledObjectPolicy(IOptions<RabbitMqConf> optionsAccs)
        {
            _options = optionsAccs.Value;
            _connection = GetConnection();
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _options.HostName,
                UserName = _options.UserName,
                Password = _options.Password,
                Port = _options.Port,
                VirtualHost = _options.VirtualHostUrl
            };
            return factory.CreateConnection();
        }

        public IModel Create()
        {
            return _connection.CreateModel();
        }

        public bool Return(IModel obj)
        {
            if (obj.IsOpen)
                return true;
            else
            {
                obj?.Dispose();
                return false;
            }
        }
    }
}
