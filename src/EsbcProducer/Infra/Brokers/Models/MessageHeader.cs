using System;

namespace EsbcProducer.Infra.Brokers.Models
{
    public class MessageHeader
    {
        public MessageHeader()
        {
            MessageId = Guid.NewGuid();
            CreationDateTime = DateTimeOffset.UtcNow;
        }

        public Guid MessageId { get; set; }

        public DateTimeOffset CreationDateTime { get; set; }
    }
}