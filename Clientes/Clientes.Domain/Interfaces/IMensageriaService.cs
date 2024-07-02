using Clientes.Domain.Models;

namespace Clientes.Domain.Interfaces
{
    public interface IMensageriaService
    {
        Task Publish(Cliente cliente);
    }
}
