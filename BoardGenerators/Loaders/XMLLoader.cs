using System.IO;
using System.Xml.Serialization;

namespace BoardGenerators.Loaders
{
    public class XmlLoader<TSerializable>
    {
        public TSerializable LoadConfigurationFromFile(string filepath)
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var reader = new StreamReader(filepath);
            var conf = (TSerializable) serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }

        public TSerializable LoadConfigurationFromText(string filecontent)
        {
            var serializer = new XmlSerializer(typeof(TSerializable));
            var reader = new StringReader(filecontent);
            var conf = (TSerializable) serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }
    }
}