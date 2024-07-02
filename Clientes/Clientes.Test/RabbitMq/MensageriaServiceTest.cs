using AutoMapper;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Models;
using Clientes.Infra.RabbitMQ.Mappings;
using Clientes.Infra.RabbitMQ.Models.cs;
using Clientes.Infra.RabbitMQ.Service;
using Moq;

namespace Clientes.Test.RabbitMq
{
    public class MensageriaServiceTest
    {
        private readonly Mock<IProducer<Message>> _producer;
        private readonly IMapper _mapper;
        private readonly MensageriaService _service;

        public MensageriaServiceTest()
        {
            _producer = new Mock<IProducer<Message>>();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MessageMapper()));
            _mapper = new Mapper(configuration);

            _service = new MensageriaService(_producer.Object, _mapper);
        }

        [Fact]
        public async Task DadoClienteMensagemSeraPublicadaComSucesso()
        {
            var cliente = new Cliente(12345, "Fulano", "12345678900", Convert.ToDateTime("2000-02-04"));

            _producer.Setup(x => x.Publish(It.Is<Message>(m => m.Id == cliente.Id && m.Nome == cliente.Nome))).Returns(Task.CompletedTask);

            await _service.Publish(cliente);

            _producer.Verify(p => p.Publish(It.Is<Message>(m => m.Id == cliente.Id)), Times.Once);
        }

        [Fact]
        public async Task DadoErroNoPublisherMensagemNaoSeraPublicadaEResilienciaSeraUsada()
        {
            var cliente = new Cliente(12345, "Fulano", "12345678900", Convert.ToDateTime("2000-02-04"));

            _producer.Setup(x => x.Publish(It.Is<Message>(m => m.Id == cliente.Id && m.Nome == cliente.Nome))).ThrowsAsync(new ApplicationException());

            await Assert.ThrowsAsync<ApplicationException>(async () => await _service.Publish(cliente));

            _producer.Verify(p => p.Publish(It.Is<Message>(m => m.Id == cliente.Id)), Times.Exactly(4));
        }

    }
}
