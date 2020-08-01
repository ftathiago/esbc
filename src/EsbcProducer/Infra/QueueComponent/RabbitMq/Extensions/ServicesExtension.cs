using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers.Impl;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Extensions
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