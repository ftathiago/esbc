using EsbcProducer.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        public IProducer _producer { get; }

        public Worker(ILogger<Worker> logger, IProducer producer)
        {
            _producer = producer;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation($"Worker running at: { DateTimeOffset.Now}");
                await _producer.Send(new
                {
                    message = $"Producing test at {DateTimeOffset.Now}",
                });
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
