using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Collections.Generic;
using Serilog;
using VPBANK.RMD.Utils.AuditLog;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using VPBANK.RMD.Utils.Common.Shared;
using System.Linq;
using VPBANK.RMD.API.Settings.Sections;

namespace VPBANK.RMD.Utils.Notification.Receiver
{
    public class RabbitMqReceiver : IDisposable
    {
        private readonly IRabbitMQPersistentConnection _persistentConnection;
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;

        private IModel _consumerChannel;
        private string _queueName;

        public RabbitMqReceiver(IRabbitMQPersistentConnection persistentConnection, IServiceCollection services, IConfiguration config, string queueName = null)
        {
            _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
            _services = services;
            _config = config;
            _queueName = queueName;
        }

        public IModel CreateConsumerChannel(bool hasRouteKeyFixed)
        {
            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            var channel = _persistentConnection.CreateModel();

            var exchangeName = _config["RabbitMq:NotiExchange"];
            var routeKey = string.Empty;
            if (hasRouteKeyFixed) routeKey = _config["RabbitMq:RouterKey"];
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Fanout, durable: true, autoDelete: false, arguments: null);

            // Default if not declare queue
            //_queueName = channel.QueueDeclare().QueueName;
            //_queueName = _config["RabbitMq:NotiQueue"];

            // Queue name auto generated when exchange created
            string pathQueueName = Environment
                .CurrentDirectory
                .Substring(Path.GetPathRoot(Environment.CurrentDirectory).Length)
                .Replace(SpecificSystems.BACK_SLASH, SpecificSystems.UNDERSCORE)
                .Replace(SpecificSystems.WHITE_SPACE, SpecificSystems.UNDERSCORE);
            _queueName = $"Webapp_Recive-{Environment.MachineName}-{pathQueueName}";

            channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: _queueName, exchange: exchangeName, routingKey: routeKey, arguments: null);
            Console.WriteLine(" [*] Waiting for messages.");
            Log.Information(" [*] Waiting for messages.");

            // Create event when something receive
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += ReceivedEvent;

            // Callback exception
            channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer, consumerTag: string.Empty, noLocal: false, exclusive: false, arguments: null);
            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel(hasRouteKeyFixed);
            };
            return channel;
        }

        private void ReceivedEvent(object sender, BasicDeliverEventArgs e)
        {
            //var serviceProvider = _services.BuildServiceProvider();
            //var _notificationService = (INotificationService)serviceProvider.GetService(typeof(INotificationService));
            //var _elasticProvider = (INotiElasticProvider<NotificationHistory>)serviceProvider.GetService(typeof(INotiElasticProvider<NotificationHistory>));

            //byte[] body = e.Body.ToArray();
            //var sBody = Encoding.UTF8.GetString(body);
            //var notify = JsonConvert.DeserializeObject<dynamic>(sBody, JsonSetting.ConfigFormat);
            //Log.Information($"RouterKey event: {JsonConvert.SerializeObject(notify, Formatting.Indented)}");
            //notify.recvTime = DateTime.Now;
            //Notification noti = JsonConvert.DeserializeObject<Notification>(JsonConvert.SerializeObject(notify));
            //var routerKey = e.RoutingKey;
            //Log.Information($"RouterKey event: {routerKey}");
            //routerKey = noti.routingKey;
            //Log.Information($"RouterKey config: {routerKey}");
            //var subscribers = _notificationService.FindAllSubscribersByRoutekey(routerKey).Distinct().ToList();

            //try
            //{
            //    // send to elastic search
            //    foreach (var subscriber in subscribers)
            //    {
            //        var notiHis = new NotificationHistory
            //        {
            //            id = Guid.NewGuid().ToString(),
            //            recvTime = noti.recvTime,
            //            type = noti.type,
            //            messageType = noti.messageType,
            //            receiver = subscriber,
            //            payload = noti.payload,
            //            seen = false,
            //            extractInfo = new Dictionary<string, object>()
            //        };
            //        _elasticProvider.AddLog(notiHis);
            //    }

            //    // increment noti for user
            //    _notificationService.IncrementCount(subscribers);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error($"Error logs notifications received: {ex.Message}");
            //    Log.Error($"Error logs notifications received: {ex.StackTrace}");
            //}
        }

        public void PublishFeedback(string _queueName, object publishModel, IDictionary<string, object> headers)
        {

            if (!_persistentConnection.IsConnected)
                _persistentConnection.TryConnect();

            using (var channel = _persistentConnection.CreateModel())
            {
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                IBasicProperties properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                properties.DeliveryMode = 2;
                properties.Headers = headers;
                properties.Expiration = "36000000";
                properties.ContentType = "text/plain";

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: "", routingKey: _queueName, mandatory: true, basicProperties: properties, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(publishModel)));
                channel.WaitForConfirmsOrDie();

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                    //implement ack handle
                };
                channel.ConfirmSelect();
            }
        }

        public void Dispose()
        {
            if (_consumerChannel != null)
                _consumerChannel.Dispose();
        }
    }
}
