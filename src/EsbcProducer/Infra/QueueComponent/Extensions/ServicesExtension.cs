using EsbcProducer.Infra.Kafka.Extensions;
using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Infra.QueueComponent.Abstractions.Impl;
using EsbcProducer.Infra.QueueComponent.Abstractions.Providers;
using EsbcProducer.Infra.QueueComponent.Abstractions.Providers.Impl;
using EsbcProducer.Infra.QueueComponent.Configurations;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace EsbcProducer.Infra.QueueComponent.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddQueueDependencies(
            this IServiceCollection services,
            Action<QueueConfiguration> configure) =>
            services
                .AddConfiguration(configure)
                .AddScoped<IProducer, Producer>()
                .AddScoped<IProducerProvider, ProducerProvider>()
                .AddKafkaDependencies()
                .AddRabbitMqDependencies();

        private static IServiceCollection AddConfiguration(
            this IServiceCollection services,
            Action<QueueConfiguration> configure)
        {
            var queueConfiguration = new QueueConfiguration();
            configure(queueConfiguration);

            return services
                .AddSingleton<QueueConfiguration>(queueConfiguration);
        }
    }
}