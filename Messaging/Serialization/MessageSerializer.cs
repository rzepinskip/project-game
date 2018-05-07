using Common.Interfaces;
using Communication;

namespace Messaging.Serialization
{
    public class MessageSerializer : IMessageDeserializer
    {
        private static MessageSerializer _instance;
        private readonly ExtendedMessageXmlDeserializer _requestSerializer;
        private readonly ExtendedXmlSerializer _xmlSerializer;

        private MessageSerializer()
        {
            _xmlSerializer = new ExtendedXmlSerializer(Constants.DefaultMessagingNameSpace);
            _requestSerializer = new ExtendedMessageXmlDeserializer(Constants.DefaultMessagingNameSpace);
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