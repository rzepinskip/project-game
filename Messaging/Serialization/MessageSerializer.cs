using Messaging.Requests;
using Messaging.Responses;

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

        public Request DeserializeRequest(string xml)
        {
            return _requestSerializer.Deserialize(xml);
        }

        public Response DeserializeResponse(string xml)
        {
            return _xmlSerializer.DeserializeFromXml<ResponseWithData>(xml);
        }

        public string Serialize(Request request)
        {
            return _xmlSerializer.SerializeToXml(request);
        }

        public string Serialize(Response response)
        {
            return _xmlSerializer.SerializeToXml(response);
        }
    }
}