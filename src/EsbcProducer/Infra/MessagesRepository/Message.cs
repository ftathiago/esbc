namespace EsbcProducer.Infra.MessagesRepository
{
    public class Message
    {
        public Message()
        {
            Header = new MessageHeader();
        }

        public MessageHeader Header { get; set; }

        public object Payload { get; set; }
    }
}