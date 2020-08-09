using EsbcProducer.Brokers;
using EsbcProducer.Configurations;
using EsbcProducer.Infra.Brokers;
using EsbcProducer.Infra.Extensions;
using EsbcProducer.Services;
using EsbcProducer.Services.Impl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EsbcProducer.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddDependencies(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddScoped<IMessageProducer, MessageProducer>()
                .AddScoped<IMessageBroker, MessageBroker>()
                .AddInfraDependencies(configuration)
                .Configure<MessageConfig>(configuration.GetSection(MessageConfig.SectionName));
    }
}