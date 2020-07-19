using System.Threading.Tasks;
using EsbcProducer.Services;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace EsbcProducer.Infra.Kafka
{
    public class Producer : IProducer
    {
        private readonly ILogger<Producer> _logger;

        public Producer(ILogger<Producer> logger)
        {
            _logger = logger;
        }

        public Task Send(object message)
        {
            var json = JsonSerializer.Serialize(message, message.GetType());
            _logger.LogInformation($"Producing message: {json}");
            return Task.CompletedTask;
        }
    }
}