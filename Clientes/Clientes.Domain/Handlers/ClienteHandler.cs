using AutoMapper;
using Clientes.Domain.Interfaces;
using Clientes.Domain.Models;
using Clientes.Domain.Requests;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Clientes.Domain.Handlers
{
    public class ClienteHandler : IRequestHandler<ClienteRequest>
    {
        private readonly IMensageriaService _service;
        private readonly ILogger<ClienteHandler> _logger;

        public ClienteHandler(ILogger<ClienteHandler> logger, IMensageriaService service)
        {
            _service = service;
            _logger = logger;
        }

        public async Task Handle(ClienteRequest request, CancellationToken cancellationToken)
        {
            //lógica para cadastro do cliente
            _logger.LogInformation("Iniciando cadastro de cliente");
            var newId = new Random().Next();
            var clienteCadastrado = new Cliente(newId, request.Nome, request.Cpf, request.Nascimento);

            _logger.LogInformation("Publicando mensagem");
            await _service.Publish(clienteCadastrado);
        }
    }
}
