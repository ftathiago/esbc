using Confluent.Kafka;
using Microsoft.Extensions.Configuration;
using System;

namespace EsbcProducer.Infra.Kafka.Factories.Impl
{
    public class ProducerProvider :
        IProducerProvider,
        IDisposable
    {
        private readonly ProducerBuilder<Null, string> _producerBuild;
        private bool disposedValue;
        private IProducer<Null, string> _producer;

        public ProducerProvider(IConfiguration configuration)
        {
            var bootstrapserver = configuration["Queue:Host"];
            if (bootstrapserver.Equals(string.Empty))
            {
                throw new ArgumentException("There is not Queue host configured");
            }

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = $"{bootstrapserver}:9092",
                RequestTimeoutMs = 5000,
            };
            _producerBuild = new ProducerBuilder<Null, string>(producerConfig);
        }

        public void Dispose()
        {
            // Não altere este código. Coloque o código de limpeza no método 'Dispose(bool disposing)'
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IProducer<Null, string> GetProducer()
        {
            return _producer ??= _producerBuild.Build();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Tarefa pendente: descartar o estado gerenciado (objetos gerenciados)
                }

                if (!(_producer is null))
                {
                    _producer.Dispose();
                }

                disposedValue = true;
            }
        }
    }
}