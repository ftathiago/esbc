using EsbcProducer.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IProducer _producer;

        public Worker(ILogger<Worker> logger, IProducer producer)
        {
            _producer = producer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Worker running at: { DateTimeOffset.Now}");
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _producer.Send(new
                    {
                        message = $"Producing test at {DateTimeOffset.Now}",
                    },
                    stoppingToken).ConfigureAwait(false);
                    await Task.Delay(5000, stoppingToken).ConfigureAwait(false);
                }
                catch (Exception exception)
                {
                    _logger.LogError(exception, "Error while sending message");
                }
            }
        }
    }
}
