using System.Diagnostics.CodeAnalysis;

namespace Clientes.Infra.RabbitMQ.Configuration
{
    [ExcludeFromCodeCoverage]
    public class ProducerConfig
    {

        public string HostName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string VirtualHost { get; set; } = default!;
        public string ExchangeName { get; set; } = default!;
        public int Port { get; set; }
        public string RoutingKey { get; set; } = default!;
        public string QueueNameA { get; set; } = default!;
        public string QueueNameB { get; set; } = default!;
    }
}
