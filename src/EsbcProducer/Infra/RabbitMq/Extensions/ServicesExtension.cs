using EsbcProducer.Infra.Kafka.Extensions;
using EsbcProducer.Infra.RabbitMq.Producers;
using EsbcProducer.Infra.RabbitMq.Producers.Impl;
using EsbcProducer.Infra.RabbitMq.Providers;
using EsbcProducer.Infra.RabbitMq.Providers.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Infra.RabbitMq.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddRabbitMqDependencies(
            this IServiceCollection services) =>
            services
                .AddScoped<IChannelProvider, ChannelProvider>()
                .AddScoped<IConnectionProvider, ConnectionProvider>()
                .AddScoped<IRabbitMqProducerWrapped, ProducerWrapped>();
    }
}