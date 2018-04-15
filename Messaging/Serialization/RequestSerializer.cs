﻿using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
using Messaging.Requests;

namespace Messaging.Serialization
{
    public class RequestSerializer : ExtendedXmlSerializer
    {
        private readonly Dictionary<string, XmlSerializer> _serializers;

        public RequestSerializer(string xmlNamespace) : base(xmlNamespace)
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
                }
            };
        }

        public Request Deserialize(string xml)
        {
            var stream = new StringReader(xml);
            var document = XDocument.Load(stream);

            var name = ReadRootName(document);
            var serializer = _serializers[name];

            if (document.Root == null) return null;

            return serializer.Deserialize(document.Root.CreateReader()) as Request;
        }

        private string ReadRootName(XDocument document)
        {
            var node = document.Root;

            return node?.Name.LocalName;
        }
    }
}