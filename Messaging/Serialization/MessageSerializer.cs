using Common.Interfaces;
using Communication;

namespace Messaging.Serialization
{
    public class MessageSerializer : IMessageDeserializer
    {
        private const string DefaultNameSpace = "https://se2.mini.pw.edu.pl/17-results/";

        private static MessageSerializer _instance;
        private readonly ExtendedMessageXmlDeserializer _requestSerializer;
        private readonly ExtendedXmlSerializer _xmlSerializer;

        private MessageSerializer()
        {
            _xmlSerializer = new ExtendedXmlSerializer(DefaultNameSpace);
            _requestSerializer = new ExtendedMessageXmlDeserializer(DefaultNameSpace);
        }

        public static MessageSerializer Instance => _instance ?? (_instance = new MessageSerializer());


        public IMessage Deserialize(string xml)
        {
            return _requestSerializer.Deserialize(xml);
        }

        public string Serialize(Message message)
        {
            return _xmlSerializer.SerializeToXml(message);
        }
    }
}