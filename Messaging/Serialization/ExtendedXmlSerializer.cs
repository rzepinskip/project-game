using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Messaging.Serialization
{
    public class ExtendedXmlSerializer
    {
        protected readonly string DefaultNamespace;

        public ExtendedXmlSerializer(string xmlNamespace)
        {
            DefaultNamespace = xmlNamespace;
        }

        private static XmlSerializer GetXmlSerializer(Type type, string xmlNamespace)
        {
            return new XmlSerializer(type, new XmlAttributeOverrides(), new Type[] { },
                new XmlRootAttribute {Namespace = xmlNamespace}, "");
        }

        public XmlSerializer GetDefaultXmlSerializer(Type type)
        {
            return GetXmlSerializer(type, DefaultNamespace);
        }

        public string SerializeToXml<T>(T value)
        {
            if (value == null) return string.Empty;

            var ns = new XmlSerializerNamespaces();
            ns.Add("", DefaultNamespace);

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

            using (var reader = new StringReader(xml))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        public T DeserializeFromXmlFile<T>(string filePath)
        {
            var serializer = GetDefaultXmlSerializer(typeof(T));

            using (var reader = new StreamReader(filePath))
            {
                return (T) serializer.Deserialize(reader);
            }
        }

        private class Utf8EncodedStringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}