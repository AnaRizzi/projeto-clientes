using CartaoCredito.Models;
using RabbitMQ.Client.Events;

namespace CartaoCredito.Interfaces
{
    public interface IConsumer
    {
        void GetMessage(Action<Message, BasicDeliverEventArgs> dequeue);
        void ProcessFinishMessage(BasicDeliverEventArgs arg);
        void RetryMessage(BasicDeliverEventArgs arg);
    }
}
