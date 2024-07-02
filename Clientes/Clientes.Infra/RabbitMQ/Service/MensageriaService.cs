using AutoMapper;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Models;
using Clientes.Infra.RabbitMQ.Models.cs;
using Polly;

namespace Clientes.Infra.RabbitMQ.Service
{
    public class MensageriaService : IMensageriaService
    {
        private readonly IProducer<Message> _producer;
        private readonly IMapper _mapper;
        private readonly AsyncPolicy _retryPolicy;


        public MensageriaService(IProducer<Message> producer, IMapper mapper)
        {
            _producer = producer;
            _mapper = mapper;
            _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        public async Task Publish(Cliente cliente)
        {
            var mensagem = _mapper.Map<Message>(cliente);

            await _retryPolicy.ExecuteAsync(() => _producer.Publish(mensagem));

        }
    }
}
