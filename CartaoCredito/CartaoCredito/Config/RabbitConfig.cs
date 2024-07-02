namespace CartaoCredito.Config
{
    public class RabbitConfig
    {
        public string HostName { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string VirtualHost { get; set; } = default!;
        public string QueueName { get; set; } = default!;
        public string ExchangeName { get; set; } = default!;
        public int Port { get; set; }
        public string DeadLetterQueue { get; set; } = default!;
        public string DeadLetterExchange { get; set; } = default!;
    }
}
