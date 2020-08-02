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
            this IServiceCollection services,
            QueueConfiguration queueConfiguration) =>
            services
                .AddScoped<IChannelProvider, ChannelProvider>()
                .AddScoped<IRabbitMqProducerWrapped, ProducerWrapped>()
                .AddScoped<IRabbitMqConnectionKeeper, RabbitMqConnectionKeeper>()
                .AddTransient<IConnectionFactory>((_) =>
                    new ConnectionFactory
                    {
                        HostName = queueConfiguration.HostName,
                        Port = queueConfiguration.Port,
                        UserName = queueConfiguration.User,
                        Password = queueConfiguration.Password,
                    });
    }
}