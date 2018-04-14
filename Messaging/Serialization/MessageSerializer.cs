using Messaging.Requests;

namespace Messaging.Serialization
{
    public class MessageSerializer
    {
        private const string DefaultNameSpace = "https://se2.mini.pw.edu.pl/17-results/";

        private static MessageSerializer _instance;
        private readonly RequestSerializer _requestSerializer;
        private readonly ExtendedXmlSerializer _xmlSerializer;

        private MessageSerializer()
        {
            _xmlSerializer = new ExtendedXmlSerializer(DefaultNameSpace);
            _requestSerializer = new RequestSerializer(DefaultNameSpace);
        }

        public static MessageSerializer Instance => _instance ?? (_instance = new MessageSerializer());


        public Message Deserialize<TMessage>(string xml) where TMessage : Message
        {
            if (typeof(TMessage) == typeof(Request))
                return _requestSerializer.Deserialize(xml);

            return _xmlSerializer.DeserializeFromXml<TMessage>(xml);
        }

        public string Serialize(Message message)
        {
            return _xmlSerializer.SerializeToXml(message);
        }
    }
}