using System.IO;
using System.Xml.Serialization;
using GameMaster.Tests.BoardConfigurationGenerator;

namespace GameMaster.Configuration
{
    public class BoardConfigurationLoader
    {
        public BoardConfiguration LoadConfigurationFromFile(string filepath)
        {
            var serializer = new XmlSerializer(typeof(BoardConfiguration));
            var reader = new StreamReader(filepath);
            var conf = (BoardConfiguration) serializer.Deserialize(reader);
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