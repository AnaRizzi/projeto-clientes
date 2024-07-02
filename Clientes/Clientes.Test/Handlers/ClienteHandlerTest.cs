using Clientes.Domain.Handlers;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Models;
using Clientes.Domain.Requests;
using Clientes.Test.ModelFake;
using Microsoft.Extensions.Logging;
using Moq;

namespace Clientes.Test.Handlers
{
    public class ClienteHandlerTest
    {
        private readonly Mock<IMensageriaService> _service;
        private readonly Mock<ILogger<ClienteHandler>> _logger;
        private readonly ClienteHandler _handler;
        private ClienteRequest _request;

        public ClienteHandlerTest()
        {
            _service = new Mock<IMensageriaService>();
            _logger = new Mock<ILogger<ClienteHandler>>();

            _handler = new ClienteHandler(_logger.Object, _service.Object);

            _request = ClienteRequestFake.RequestValida();
        }

        [Fact]
        public void ClienteCadastradoEMensagemPublicadaComSucesso()
        {
            _service.Setup(x => x.Publish(It.Is<Cliente>(c => c.Nome == _request.Nome && c.Cpf == _request.Cpf && c.Nascimento == _request.Nascimento))).Returns(Task.CompletedTask);

            var result = _handler.Handle(_request, CancellationToken.None);

            Assert.Equal(Task.CompletedTask, result);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("Iniciando cadastro de cliente", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("Publicando mensagem", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }

        [Fact]
        public void ClienteCadastradoEErroAoPublicarMensagem()
        {
            _service.Setup(x => x.Publish(It.Is<Cliente>(c => c.Nome == _request.Nome && c.Cpf == _request.Cpf && c.Nascimento == _request.Nascimento))).ThrowsAsync(new Exception("Erro ao publicar mensagem"));

            Assert.ThrowsAsync<Exception>(
                () => _handler.Handle(_request, CancellationToken.None)
            );

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("Iniciando cadastro de cliente", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);

            _logger.Verify(
                logger => logger.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((o, t) =>
                        string.Equals("Publicando mensagem", o.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
        }
    }
}
