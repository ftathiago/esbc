using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Worker
{
    public class ProducerWorker : BackgroundService
    {
        private readonly ILogger<ProducerWorker> _logger;
        private readonly IProletarian _proletarian;

        public ProducerWorker(ILogger<ProducerWorker> logger, IProletarian proletarian)
        {
            _proletarian = proletarian;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _proletarian.DoWork(stoppingToken);
                    await Task.Delay(5000, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error while sending message");
                }
            }
        }
    }
}
