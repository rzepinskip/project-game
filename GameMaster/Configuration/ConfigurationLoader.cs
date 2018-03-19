using System.IO;
using System.Xml.Serialization;

namespace GameMaster.Configuration
{
    public class ConfigurationLoader
    {
        public GameConfiguration LoadConfigurationFromFile(string filepath)
        {
            var serializer = new XmlSerializer(typeof(GameConfiguration));
            var reader = new StreamReader(filepath);
            var conf = (GameConfiguration) serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }

        public GameConfiguration LoadConfigurationFromText(string filecontent)
        {
            var serializer = new XmlSerializer(typeof(GameConfiguration));
            var reader = new StringReader(filecontent);
            var conf = (GameConfiguration) serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }
    }
}