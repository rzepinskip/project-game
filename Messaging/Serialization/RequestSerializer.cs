using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Messaging.Requests;

namespace Messaging.Serialization
{
    public class RequestSerializer
    {
        private static RequestSerializer _instance;
        private static Dictionary<string, XmlSerializer> _serializers;

        private RequestSerializer()
        {
            _serializers = new Dictionary<string, XmlSerializer>
            {
                {
                    DiscoverRequest.XmlRootName, 
                    ExtendedXmlSerializer.GetDefaultXmlSerializer(typeof(DiscoverRequest))
                },
                {
                    MoveRequest.XmlRootName, 
                    ExtendedXmlSerializer.GetDefaultXmlSerializer(typeof(MoveRequest))
                },
                {
                    PickUpPieceRequest.XmlRootName,
                    ExtendedXmlSerializer.GetDefaultXmlSerializer(typeof(PickUpPieceRequest))
                },
                {
                    PlacePieceRequest.XmlRootName,
                    ExtendedXmlSerializer.GetDefaultXmlSerializer(typeof(PlacePieceRequest))
                },
                {
                    TestPieceRequest.XmlRootName, 
                    ExtendedXmlSerializer.GetDefaultXmlSerializer(typeof(TestPieceRequest))
                }
            };
        }

        public static RequestSerializer Instance => _instance ?? (_instance = new RequestSerializer());

        public Request Deserialize(string xml)
        {
            var stream = new StringReader(xml);
            var document = XDocument.Load(stream);

            var name = ReadRootName(document);

            var serializer = _serializers[name];


            if (document.Root == null) return null;

            var result = serializer.Deserialize(document.Root.CreateReader()) as Request;
            return result;
        }

        private string ReadRootName(XDocument document)
        {
            var node = document.Root;

            return node?.Name.LocalName;
        }
    }
}