namespace Messaging.Serialization
{
    public class MessageSerializer
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        private static MessageSerializer _instance;
        private readonly ExtendedMessageXmlDeserializer _messageDeserializer;
        private readonly ExtendedXmlSerializer _xmlSerializer;

        private MessageSerializer()
        {
            _xmlSerializer = new ExtendedXmlSerializer(DefaultNamespace);
            _messageDeserializer = new ExtendedMessageXmlDeserializer(DefaultNamespace);
        }

        public static MessageSerializer Instance => _instance ?? (_instance = new MessageSerializer());


        public Message Deserialize(string xml)
        {
            return _messageDeserializer.Deserialize(xml);
        }

        public string Serialize(Message message)
        {
            return _xmlSerializer.SerializeToXml(message);
        }
    }
}