using System;

namespace EsbcProducer.Infra.MessagesRepository
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