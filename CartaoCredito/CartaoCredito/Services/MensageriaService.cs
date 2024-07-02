using CartaoCredito.Interfaces;
using CartaoCredito.Models;
using RabbitMQ.Client.Events;

namespace CartaoCredito.Services
{
    public class MensageriaService : IMensageriaService
    {
        private readonly IConsumer _consumer;
        private readonly ILogger<MensageriaService> _logger;


        public MensageriaService(ILogger<MensageriaService> logger, IConsumer consumer)
        {
            _logger = logger;
            _consumer = consumer;
        }

        public void Execute()
        {
            _consumer.GetMessage(ProcessMessage);
        }

        public void ProcessMessage(Message message, BasicDeliverEventArgs arg)
        {
            _logger.LogInformation("Processando a mensagem do cliente " + message.Id);

            GerarCartaoCredito(message);

            _consumer.ProcessFinishMessage(arg);

            _logger.LogInformation("Processo finalizado, id: " + message.Id);
        }

        private void GerarCartaoCredito(Message message)
        {
            _logger.LogInformation("Gerando cartão de crédito para cliente " + message.Id);
        }
    }
}
