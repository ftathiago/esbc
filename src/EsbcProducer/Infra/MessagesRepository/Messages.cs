using EsbcProducer.Infra.QueueComponent.Abstractions;
using EsbcProducer.Repositories;
using System.Text.Json;
using System.Threading.Tasks;

namespace EsbcProducer.Infra.MessagesRepository
{
    public class Messages : IMessages
    {
        private readonly IProducer _producer;

        public Messages(IProducer producer)
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