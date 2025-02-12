using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;
using System;
using System.IO;
using System.Threading;

namespace VPBANK.RMD.Utils.Notification.Receiver
{
    public interface IRabbitMQPersistentConnection : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();

        void CreateConsumerChannel();

        void Disconnect();
    }

    public class RabbitMQPersistentConnection : IRabbitMQPersistentConnection
    {
        private readonly IConnectionFactory _factory;
        private readonly IServiceCollection _services;
        private readonly IConfiguration _config;

        RabbitMqReceiver _eventBusRabbitMQ;
        IConnection _connection;
        bool _disposed;


        public RabbitMQPersistentConnection(IConnectionFactory factory)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            if (!IsConnected)
                TryConnect();
        }

        public RabbitMQPersistentConnection(IConnectionFactory factory, IServiceCollection services, IConfiguration config)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _services = services;
            _config = config;
            if (!IsConnected)
                TryConnect();
        }

        public void CreateConsumerChannel()
        {
            if (!IsConnected)
                TryConnect();
            
            _eventBusRabbitMQ = new RabbitMqReceiver(this, _services, _config);
            _eventBusRabbitMQ.CreateConsumerChannel(true);
        }

        public void Disconnect()
        {
            if (_disposed)
                return;
            Dispose();
        }


        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                Log.Error("No RabbitMQ connections are available to perform this action");
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }
            Log.Information("RabbitMQ connections successful");
            return _connection.CreateModel();
        }

        public void Dispose()
        {
            if (_disposed) return;

            _disposed = true;

            try
            {
                _connection.Dispose();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public bool TryConnect()
        {
            try
            {
                Console.WriteLine("RabbitMQ Client is trying to connect");
                Log.Information("RabbitMQ Client is trying to connect");
                _connection = _factory.CreateConnection();
            }
            catch (BrokerUnreachableException ex)
            {
                Thread.Sleep(5000);
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                Log.Error(ex.InnerException.Message);
                Log.Error("RabbitMQ Client is trying to reconnect");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.WriteLine("RabbitMQ Client is trying to reconnect");
                _connection = _factory.CreateConnection();
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown;
                _connection.CallbackException += OnCallbackException;
                _connection.ConnectionBlocked += OnConnectionBlocked;

                Log.Information($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");
                Console.WriteLine($"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events");

                return true;
            }
            else
            {
                //  implement send warning email here
                //-----------------------
                Log.Warning("FATAL ERROR: RabbitMQ connections could not be created and opened");
                Console.WriteLine("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }

        }

        private void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed) return;
            Log.Warning("A RabbitMQ connection is shutdown. Trying to re-connect...");
            Console.WriteLine("A RabbitMQ connection is shutdown. Trying to re-connect...");
            TryConnect();
        }

        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed) return;
            Log.Warning("A RabbitMQ connection throw exception. Trying to re-connect...");
            Console.WriteLine("A RabbitMQ connection throw exception. Trying to re-connect...");
            TryConnect();
        }

        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed) return;
            Log.Warning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            Console.WriteLine("A RabbitMQ connection is on shutdown. Trying to re-connect...");
            TryConnect();
        }
    }
}
