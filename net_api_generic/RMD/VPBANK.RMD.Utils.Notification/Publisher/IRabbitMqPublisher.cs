using Microsoft.Extensions.ObjectPool;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Text;

namespace VPBANK.RMD.Utils.Notification.Publisher
{
    public interface IRabbitMqPublisher
    {
        void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
        void Publish<T>(T message, string exchangeName, string exchangeType, string queueName, string routeKey) where T : class;
        void PublishAsync<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class;
        void PublishAsync<T>(T message, string exchangeName, string exchangeType, string queueName, string routeKey) where T : class;
    }

    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly DefaultObjectPool<IModel> _objectPool;

        public RabbitMqPublisher(IPooledObjectPolicy<IModel> objectPolicy)
        {
            _objectPool = new DefaultObjectPool<IModel>(objectPolicy, Environment.ProcessorCount * 2);
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();
            try
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true, autoDelete: false, arguments: null);
                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;
                channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, mandatory: true, basicProperties: properties, body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public void Publish<T>(T message, string exchangeName, string exchangeType, string queueName, string routeKey) where T : class
        {
            if (message == null)
                return;

            var channel = _objectPool.Get();
            try
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType, durable: true, autoDelete: false, arguments: null);
                channel.QueueDeclare(queue: queueName, durable: true, exclusive: false, autoDelete: true, arguments: null);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: routeKey, arguments: null);

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: exchangeName, routingKey: routeKey, mandatory: true, basicProperties: CreateBasicProperties(channel), body: Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message)));
                channel.WaitForConfirmsOrDie();
                channel.ConfirmSelect();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                _objectPool.Return(channel);
            }
        }

        public void PublishAsync<T>(T message, string exchangeName, string exchangeType, string routeKey) where T : class
        {
            throw new NotImplementedException();
        }

        public void PublishAsync<T>(T message, string exchangeName, string exchangeType, string queueName, string routeKey) where T : class
        {
            throw new NotImplementedException();
        }

        private IBasicProperties CreateBasicProperties(IModel channel, IDictionary<string, object> headers = null)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.DeliveryMode = 2;
            properties.Headers = headers;
            properties.Expiration = "36000000";
            properties.ContentType = MediaTypeNames.Text.Plain;
            return properties;
        }
    }
}
