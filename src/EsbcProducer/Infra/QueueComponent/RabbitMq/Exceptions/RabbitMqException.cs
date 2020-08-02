using System;
using System.Runtime.Serialization;

namespace EsbcProducer.Infra.QueueComponent.RabbitMq.Exceptions
{
    [Serializable]
    public class RabbitMqException : Exception
    {
        public RabbitMqException()
        {
        }

        public RabbitMqException(string message)
            : base(message)
        {
        }

        public RabbitMqException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected RabbitMqException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}