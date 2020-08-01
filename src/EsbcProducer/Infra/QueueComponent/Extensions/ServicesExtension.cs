using EsbcProducer.Infra.Kafka.Extensions;
using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Infra.QueueComponent.Abstractions.Impl;
using EsbcProducer.Infra.QueueComponent.Abstractions.Providers;
using EsbcProducer.Infra.QueueComponent.Abstractions.Providers.Impl;
using EsbcProducer.Infra.QueueComponent.Configurations;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Extensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Infra.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddInfraDependencies(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddScoped<IProducer, Producer>()
                .AddScoped<IProducerProvider, ProducerProvider>()
                .AddConfiguration(configuration)
                .AddKafkaDependencies()
                .AddRabbitMqDependencies();

        private static IServiceCollection AddConfiguration(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddSingleton<QueueConfiguration>(QueueConfiguration.From(configuration));
    }
}