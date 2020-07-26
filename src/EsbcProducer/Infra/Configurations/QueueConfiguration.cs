using EsbcProducer.Extensions;
using Microsoft.Extensions.Configuration;

namespace EsbcProducer.Infra.Configurations
{
    public class QueueConfiguration
    {
        public QueueConfiguration()
        {
            KafkaConfiguration = new KafkaConfiguration();
            HostName = "localhost";
            Port = 9092;
            QueueMechanism = QueueMechanism.Kafka;
        }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public QueueMechanism QueueMechanism { get; set; }

        public KafkaConfiguration KafkaConfiguration { get; set; }

        public static QueueConfiguration From(IConfiguration configuration)
        {
            var port = int.Parse(configuration["QueueConfiguration:HostName"]);

            return new QueueConfiguration
            {
                HostName = configuration["QueueConfiguration:HostName"] ?? "localhost",
                Port = port > 0 ? port : 9092,
                QueueMechanism = configuration["QueueConfiguration:QueueMechanism"].Parse<QueueMechanism>(),
                KafkaConfiguration = KafkaConfiguration.From(configuration),
            };
        }
    }
}