using PropostaCredito.Interfaces;
using PropostaCredito.Models;
using RabbitMQ.Client.Events;

namespace PropostaCredito.Services
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

            GerarPropostaCredito(message);

            _consumer.ProcessFinishMessage(arg);

            _logger.LogInformation("Processo finalizado, id: " + message.Id);
        }

        private void GerarPropostaCredito(Message message)
        {
            _logger.LogInformation("Gerando proposta de crédito para cliente " + message.Id);
        }
    }
}
