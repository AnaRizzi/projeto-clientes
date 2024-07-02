using PropostaCredito.Interfaces;

namespace PropostaCredito
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMensageriaService _service;

        public Worker(ILogger<Worker> logger, IMensageriaService service)
        {
            _logger = logger;
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                
                _service.Execute();

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}
