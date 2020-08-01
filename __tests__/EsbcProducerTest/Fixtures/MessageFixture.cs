using System.Text;
using System.Text.Json;

namespace EsbcProducer.Fixtures
{
    public class MessageFixture
    {
        private const string MESSAGE = @"{""message"":""test""}";

        public string GetMessageString() =>
            MESSAGE;

        public byte[] GetMesageUTF8() =>
            Encoding.UTF8.GetBytes(MESSAGE);

        public object GetMessageObject() =>
            JsonSerializer.Deserialize(MESSAGE, typeof(object));
    }
}