using EsbcProducer.Infra.QueueComponent.Configurations;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Producers.Impl;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers;
using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers.Impl;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection AddRabbitMqDependencies(
            this IServiceCollection services) =>
            services
                .AddScoped<IChannelProvider, ChannelProvider>()
                .AddScoped<IRabbitMqProducerWrapped, ProducerWrapped>()
                .AddScoped<IRabbitMqConnectionKeeper, RabbitMqConnectionKeeper>()
                .AddTransient<IConnectionFactory>((services) =>
                {
                    var queueConfig = services.GetService<QueueConfiguration>();
                    return new ConnectionFactory
                    {
                        HostName = queueConfig.HostName,
                        Port = queueConfig.Port,
                        UserName = queueConfig.User,
                        Password = queueConfig.Password,
                    };
                });
    }
}