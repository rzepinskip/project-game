using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Messaging.Requests;

namespace Messaging
{
    public class RequestSerializer
    {
        private static RequestSerializer _instance;
        private static Dictionary<string, XmlSerializer> _serializers;

        private RequestSerializer()
        {
            _serializers = new Dictionary<string, XmlSerializer>
            {
                {DiscoverRequest.XmlRootName, XmlExtensions.GetDefaultXmlSerializer<DiscoverRequest>()},
                {MoveRequest.XmlRootName, XmlExtensions.GetDefaultXmlSerializer<MoveRequest>()},
                {PickUpPieceRequest.XmlRootName,XmlExtensions.GetDefaultXmlSerializer<PickUpPieceRequest>()},
                {PlacePieceRequest.XmlRootName, XmlExtensions.GetDefaultXmlSerializer<PlacePieceRequest>()},
                {TestPieceRequest.XmlRootName, XmlExtensions.GetDefaultXmlSerializer<TestPieceRequest>()},
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

    public static class XmlExtensions
    {
        private const string DefaultNamespace = "https://se2.mini.pw.edu.pl/17-results/";

        public static XmlSerializer GetDefaultXmlSerializer<T>()
        {
            return GetXmlSerializer<T>(DefaultNamespace);
        }

        private static XmlSerializer GetXmlSerializer<T>(string namepsace)
        {
            return new XmlSerializer(typeof(T), new XmlAttributeOverrides(), new Type[]{}, new XmlRootAttribute { Namespace = namepsace}, "");
        }

        public static string SerializeToXml<T>(this T value)
        {
            if (value == null) return string.Empty;
 
            var ns = new XmlSerializerNamespaces();
            ns.Add("", DefaultNamespace);
 
            var xmlSerializer = GetDefaultXmlSerializer<T>();
 
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings { Indent = true }))
                {
                    xmlSerializer.Serialize(xmlWriter, value, ns);
                    return stringWriter.ToString();
                }
            }
        }
    }
}