﻿using EsbcProducer.Infra.QueueComponent.RabbitMq.Providers;
using RabbitMQ.Client;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Producers.Impl
{
    public class ProducerWrapped : IRabbitMqProducerWrapped
    {
        private readonly IChannelProvider _channelProvider;

        public ProducerWrapped(IChannelProvider channelProvider)
        {
            _channelProvider = channelProvider;
        }

        public Task<bool> Send(string queueName, string payload, CancellationToken stoppingToken = default) =>
            Task.Run<bool>(
                () =>
                {
                    var body = Encoding.UTF8.GetBytes(payload);

                    var channel = _channelProvider
                        .QueueDeclare(queueName)
                        .GetChannel();

                    channel.BasicPublish(
                        exchange: queueName,
                        routingKey: queueName,
                        basicProperties: null,
                        body: body);
                    return true;
                },
                stoppingToken);
    }
}