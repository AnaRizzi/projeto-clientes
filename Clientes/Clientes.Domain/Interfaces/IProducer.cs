namespace Clientes.Domain.Interfaces
{
    public interface IProducer<T> where T : class
    {
        Task Publish(T message);
    }
}
