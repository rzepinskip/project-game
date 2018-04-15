using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Messaging.Serialization
{
    public class ExtendedXmlSerializer
    {
        private readonly string _defaultNamespace;

        public ExtendedXmlSerializer(string xmlNamespace)
        {
            _defaultNamespace = xmlNamespace;
        }

        private static XmlSerializer GetXmlSerializer(Type type, string xmlNamespace)
        {
            return new XmlSerializer(type, new XmlAttributeOverrides(), new Type[] { },
                new XmlRootAttribute {Namespace = xmlNamespace}, "");
        }

        public XmlSerializer GetDefaultXmlSerializer(Type type)
        {
            return GetXmlSerializer(type, _defaultNamespace);
        }

        public string SerializeToXml<T>(T value)
        {
            if (value == null) return string.Empty;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", _defaultNamespace);

            var xmlSerializer = GetDefaultXmlSerializer(value.GetType());

            using (var stringWriter = new Utf8EncodedStringWriter())
            {
                using (var xmlWriter = XmlWriter.Create(stringWriter, new XmlWriterSettings {Indent = true}))
                {
                    xmlSerializer.Serialize(xmlWriter, value, ns);
                    return stringWriter.ToString();
                }
            }
        }

        public T DeserializeFromXml<T>(string xml)
        {
            var serializer = GetDefaultXmlSerializer(typeof(T));
            var reader = new StringReader(xml);
            var conf = (T) serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }

        private class Utf8EncodedStringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}