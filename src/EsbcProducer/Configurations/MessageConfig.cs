namespace EsbcProducer.Configurations
{
    public class MessageConfig
    {
        public const string SectionName = "MessageConfig";
        private const string DefaultMessage = "No message";
        private const int DefaultWaitingTime = 5000;
        private string messageText;
        private int waitingTime;

        public string MessageText
        {
            get { return messageText; }
            set { messageText = value ?? DefaultMessage; }
        }

        public int WaitingTime
        {
            get { return waitingTime; }
            set { waitingTime = value <= 0 ? DefaultWaitingTime : value; }
        }
    }
}