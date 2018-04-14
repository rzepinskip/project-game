using Messaging.Requests;
using Messaging.Responses;

namespace Messaging.Serialization
{
    public static class MessageSerializer
    {
        public static Request DeserializeRequest(string xml)
        {
            return RequestSerializer.Instance.Deserialize(xml);
        }

        public static Response DeserializeResponse(string xml)
        {
            return ExtendedXmlSerializer.DeserializeFromXml<ResponseWithData>(xml);
        }

        public static string Serialize(Request request)
        {
            return ExtendedXmlSerializer.SerializeToXml(request);
        }

        public static string Serialize(Response response)
        {
            return ExtendedXmlSerializer.SerializeToXml(response);
        }
    }
}
