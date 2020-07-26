using EsbcProducer.Infra.Configurations;
using EsbcProducer.Infra.Kafka.Extensions;
using EsbcProducer.Infra.Providers;
using EsbcProducer.Infra.Providers.Impl;
using EsbcProducer.Infra.RabbitMq.Extensions;
using EsbcProducer.Repositories;
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