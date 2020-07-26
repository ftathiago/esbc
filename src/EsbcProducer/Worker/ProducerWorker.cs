using Microsoft.Extensions.DependencyInjection;
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
        private readonly IServiceProvider _services;

        public ProducerWorker(ILogger<ProducerWorker> logger, IServiceProvider services)
        {
            _logger = logger;
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _services.CreateScope();
                    var proletarian = scope.ServiceProvider.GetRequiredService<IMessageProducer>();
                    _logger.LogInformation($"Worker running at: {DateTimeOffset.Now}");
                    await DoWork(proletarian, stoppingToken);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "A not handled error occurs. Reestarting all services");
                }
            }
        }

        private async Task DoWork(IMessageProducer proletarian, CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await proletarian.DoWork(stoppingToken);
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
