using System;
using Microsoft.Extensions.Configuration;

namespace EsbcProducer.Infra.QueueComponent.Configurations
{
    public class KafkaConfiguration
    {
        private const string ConfigPathBase = "QueueConfiguration:KafkaConfiguration";

        public KafkaConfiguration()
        {
            RequestTimeoutMs = (int)TimeSpan.FromSeconds(10).TotalMilliseconds;
        }

        public int RequestTimeoutMs { get; set; }

        public static KafkaConfiguration From(IConfiguration configuration) =>
             new KafkaConfiguration
             {
                 RequestTimeoutMs = int.Parse(configuration[$"{ConfigPathBase}:RequestTimeoutMs"]),
             };
    }
}