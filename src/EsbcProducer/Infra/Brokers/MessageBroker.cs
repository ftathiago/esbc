using EsbcProducer.Brokers;
using EsbcProducer.Infra.Brokers.Models;
using EsbcProducer.Infra.QueueComponent.Abstractions;
using System.Text.Json;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.Brokers
{
    public class MessageBroker : IMessageBroker
    {
        private readonly IProducer _producer;

        public MessageBroker(IProducer producer)
        {
            _producer = producer;
        }

        public async Task Send(object payload)
        {
            var message = GetMessageObject(payload);
            await _producer.Send("topic_test", message);
        }

        private Message GetMessageObject(object payload) =>
            new Message
            {
                Payload = JsonSerializer.Serialize(payload),
            };
    }
}