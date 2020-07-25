using EsbcProducer.Infra.Kafka.Extensions;
using EsbcProducer.Infra.Providers;
using EsbcProducer.Infra.Providers.Impl;
using EsbcProducer.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Infra.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddInfraDependencies(this IServiceCollection services) =>
            services
                .AddScoped<IProducer, Producer>()
                .AddScoped<IProducerProvider, ProducerProvider>()
                .AddKafkaDependencies();
    }
}