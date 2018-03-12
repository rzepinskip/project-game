using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Shared.BoardObjects;
using Shared;
using System.Xml.Serialization;
using System.IO;

namespace GameMaster.Configuration
{
    public class ConfigurationLoader
    {
        public GameConfiguration LoadConfigurationFromFile(string filepath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameConfiguration));
            StreamReader reader = new StreamReader(filepath);
            var conf = (GameConfiguration)serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }

        public GameConfiguration LoadConfigurationFromText(string filecontent)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GameConfiguration));
            StringReader reader = new StringReader(filecontent);
            var conf = (GameConfiguration)serializer.Deserialize(reader);
            reader.Close();

            return conf;
        }
    }
}
