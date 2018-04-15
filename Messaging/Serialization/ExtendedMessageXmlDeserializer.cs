using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Messaging.Requests;
using Messaging.Responses;

namespace Messaging.Serialization
{
    public class ExtendedMessageXmlDeserializer : ExtendedXmlSerializer
    {
        private readonly Dictionary<string, XmlSerializer> _serializers;

        public ExtendedMessageXmlDeserializer(string xmlNamespace) : base(xmlNamespace)
        {
            _serializers = new Dictionary<string, XmlSerializer>
            {
                {
                    DiscoverRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(DiscoverRequest))
                },
                {
                    MoveRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(MoveRequest))
                },
                {
                    PickUpPieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(PickUpPieceRequest))
                },
                {
                    PlacePieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(PlacePieceRequest))
                },
                {
                    TestPieceRequest.XmlRootName,
                    GetDefaultXmlSerializer(typeof(TestPieceRequest))
                },
                {
                    ResponseWithData.XmlRootName,
                    GetDefaultXmlSerializer(typeof(ResponseWithData))
                }
            };
        }

        public Message Deserialize(string xml)
        {
            using (var stream = new StringReader(xml))
            {
                return ReadMessage(stream);
            }
        }

        public Message DeserializeFromFile(string filePath)
        {
            using (var stream = new StreamReader(filePath))
            {
                return ReadMessage(stream);
            }
        }

        private Message ReadMessage(TextReader stream)
        {
            var document = XDocument.Load(stream);

            var name = ReadRootName(document);
            var serializer = _serializers[name];

            Message message = null;

            if (document.Root != null)
                message = serializer.Deserialize(document.Root.CreateReader()) as Message;

            return message;
        }

        private string ReadRootName(XDocument document)
        {
            var node = document.Root;

            return node?.Name.LocalName;
        }
    }
}