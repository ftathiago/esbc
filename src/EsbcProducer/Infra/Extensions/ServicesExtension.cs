using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Infra.QueueComponent.Extensions;
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
                .AddQueueDependencies(queueConfiguration => queueConfiguration.LoadFrom(configuration));
    }
}