using EsbcProducer.Infra.Kafka.Producers;
using EsbcProducer.Infra.Kafka.Producers.Impl;
using EsbcProducer.Infra.Kafka.Providers;
using EsbcProducer.Infra.Kafka.Providers.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Infra.Kafka.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddKafkaDependencies(this IServiceCollection services) =>
            services
                .AddScoped<IProducerProvider, ProducerProvider>()
                .AddScoped<IKafkaProducerWrapped, ProducerWrapped>();
    }
}