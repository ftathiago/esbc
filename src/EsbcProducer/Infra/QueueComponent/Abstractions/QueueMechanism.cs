namespace EsbcProducer.Infra.QueueComponent.Abstractions
{
    public enum QueueMechanism
    {
        /// <summary>
        /// Default value. Will throw several exceptions
        /// </summary>
        Unknown,

        /// <summary>
        /// Specify RabbitMQ as queue mechanism
        /// </summary>
        RabbitMq,

        /// <summary>
        /// Specify Kafka as queue mechanism
        /// </summary>
        Kafka,
    }
}