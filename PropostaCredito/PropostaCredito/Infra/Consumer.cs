using PropostaCredito.Interfaces;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using PropostaCredito.Config;
using PropostaCredito.Models;
using System.Text;
using System.Text.Json;

namespace PropostaCredito.Infra
{
    public class Consumer : IConsumer
    {

        private readonly RabbitConfig _config;
        private readonly ConnectionFactory _connectionFactory;
        EventingBasicConsumer _consumer = default!;
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

        public Consumer(RabbitConfig rabbitConfig)
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
            QueueDeclare();
        }

        public void GetMessage(Action<Message, BasicDeliverEventArgs> dequeue)
        {
            _consumer = new EventingBasicConsumer(Channel);
            _consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                try
                {
                    var formatedMessage = JsonSerializer.Deserialize<Message>(message);

                    dequeue(formatedMessage!, ea);
                }
                catch
                {
                    RetryMessage(ea);
                }
            };

            Channel.BasicQos(0, 1, false);

            Channel.BasicConsume(
                queue: _config.QueueName,
                autoAck: false,
                consumer: _consumer);
        }

        private void QueueDeclare()
        {
            var args = new Dictionary<string, object>();
            args.Add("x-dead-letter-exchange", _config.DeadLetterExchange);

            Channel.QueueDeclare(
                        queue: _config.QueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: args);

            Channel.QueueBind(queue: _config.QueueName, exchange: _config.ExchangeName, "");

            CreateDeadLetter();
        }

        private void CreateDeadLetter()
        {
            Channel.QueueDeclare(
                        queue: _config.DeadLetterQueue,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);

            Channel.ExchangeDeclare(
                        exchange: _config.DeadLetterExchange,
                        type: ExchangeType.Fanout,
                        durable: true,
                        autoDelete: false,
                        arguments: null);

            Channel.QueueBind(
                _config.DeadLetterQueue,
                _config.DeadLetterExchange,
                "",
                null);
        }

        public void RetryMessage(BasicDeliverEventArgs arg)
        {
            if (GetRetryCount(arg) <= 3)
            {
                Channel.BasicNack(arg.DeliveryTag, false, false);
            }
            else
            {
                Console.WriteLine("Erro ao ler da fila! ");

                Channel.BasicAck(arg.DeliveryTag, false);
            }
        }

        private int GetRetryCount(BasicDeliverEventArgs arg)
        {
            var count = 0;

            if (arg.BasicProperties.Headers != null
                && arg.BasicProperties.Headers.ContainsKey("x-death")
                && arg.BasicProperties.Headers["x-death"] is List<object> xdeath
                && xdeath.FirstOrDefault() is Dictionary<string, object> headers)
            {
                count = Convert.ToInt32(headers["count"]);
            }

            return ++count;
        }

        public void ProcessFinishMessage(BasicDeliverEventArgs arg)
        {
            Channel.BasicAck(arg.DeliveryTag, false);
        }

    }
}
