using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Infra.QueueComponent.Extensions;
using Microsoft.Extensions.Configuration;

namespace EsbcProducer.Infra.QueueComponent.Configurations
{
    public class QueueConfiguration
    {
        private const string DefaultHostName = "127.0.0.1";
        private const int DefaultPort = 9092;
        private const int DefaultRetryCount = 5;
        private const int DefaultTimeoutMs = 5000;
        private const QueueMechanism DefaultQueueMechanism = QueueMechanism.Kafka;

        public QueueConfiguration()
        {
            HostName = DefaultHostName;
            Port = DefaultPort;
            RetryCount = DefaultRetryCount;
            TimeoutMs = DefaultTimeoutMs;
            QueueMechanism = DefaultQueueMechanism;
        }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string User { get; set; }

        public string Password { get; set; }

        public int RetryCount { get; set; }

        public int TimeoutMs { get; set; }

        public QueueMechanism QueueMechanism { get; set; }

        public static QueueConfiguration From(IConfiguration configuration) =>
            new QueueConfiguration().LoadFrom(configuration);

        public QueueConfiguration LoadFrom(IConfiguration configuration)
        {
            HostName = configuration["QueueConfiguration:HostName"] ?? DefaultHostName;
            Port = configuration["QueueConfiguration:Port"].ParseToInt(DefaultPort);
            User = configuration["QueueConfiguration:User"];
            Password = configuration["QueueConfiguration:Password"];
            RetryCount = configuration["QueueConfiguration:RetryCount"].ParseToInt(DefaultRetryCount);
            TimeoutMs = configuration["QueueConfiguration:TimeoutMs"].ParseToInt(DefaultTimeoutMs);
            QueueMechanism = configuration["QueueConfiguration:QueueMechanism"].ParseToEnum<QueueMechanism>();
            return this;
        }
    }
}