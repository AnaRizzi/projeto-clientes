using Clientes.Domain.Interfaces;
using Clientes.Infra.RabbitMQ.Configuration;
using RabbitMQ.Client;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.Json;

namespace Clientes.Infra.RabbitMQ.Infra
{
    [ExcludeFromCodeCoverage]
    public class Producer<T> : IProducer<T> where T : class
    {

        private readonly ProducerConfig _config;
        private const int persistentDeliveryMode = 2;
        private readonly ConnectionFactory _connectionFactory;
        private readonly IBasicProperties _queueProperties;
        private IModel _channel { get; set; } = default!;
        private IModel Channel
        {
            get
            {
                if (_channel is null || _channel.IsClosed)
                {
                    var connection = _connectionFactory.CreateConnection();
                    _channel = connection.CreateModel();
                }
                return _channel;
            }
        }

        public Producer(ProducerConfig rabbitConfig)
        {
            _config = rabbitConfig;
            _connectionFactory = new ConnectionFactory
            {
                HostName = rabbitConfig.HostName,
                Port = rabbitConfig.Port,
                UserName = rabbitConfig.UserName,
                Password = rabbitConfig.Password,
                VirtualHost = rabbitConfig.VirtualHost,
            };
            ExchangeDeclare();
            _queueProperties = CreateQueueProperties();
            Channel.ConfirmSelect();
        }

        public Task Publish(T message)
        {
            BasicPublish(message);
            Channel.WaitForConfirmsOrDie();
            Channel.WaitForConfirms();
            return Task.CompletedTask;
        }

        private void BasicPublish(T data)
        {
            var jsonObject = JsonSerializer.Serialize(data);
            Channel.BasicPublish(_config.ExchangeName, _config.RoutingKey, _queueProperties, Encoding.UTF8.GetBytes(jsonObject));
        }

        private void ExchangeDeclare()
        {
            if (!string.IsNullOrEmpty(_config.ExchangeName))
            {
                Channel.ExchangeDeclare(
                            exchange: _config.ExchangeName,
                            type: ExchangeType.Fanout,
                            durable: true,
                            autoDelete: false,
                            arguments: null);
            }
        }

        private IBasicProperties CreateQueueProperties()
        {
            IBasicProperties queueProperties = Channel.CreateBasicProperties();
            queueProperties.ContentType = "application/json";
            queueProperties.DeliveryMode = persistentDeliveryMode;
            queueProperties.Persistent = true;
            return queueProperties;
        }
    }
}

