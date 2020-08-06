using EsbcProducer.Configurations;
using EsbcProducer.Infra.Extensions;
using EsbcProducer.Infra.MessagesRepository;
using EsbcProducer.Repositories;
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
                .AddScoped<IMessages, Messages>()
                .AddInfraDependencies(configuration)
                .Configure<MessageConfig>(configuration.GetSection(MessageConfig.SectionName));
    }
}