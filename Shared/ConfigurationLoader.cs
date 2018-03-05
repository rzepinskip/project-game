using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Shared
{
    public class ConfigurationLoader
    {
        public GameConfiguration LoadConfiguration(XmlDocument doc)
        {
            GameConfiguration result = new GameConfiguration();

            LoadGMSettings(result, doc);

            return result;
        }

        public GameConfiguration LoadConfiguration(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            return LoadConfiguration(doc);
        }

        private void LoadGMSettings(GameConfiguration config, XmlDocument doc)
        {
            XmlNode gmSettingsNode = doc.DocumentElement;
            if (double.TryParse(gmSettingsNode.Attributes["KeepAliveInterval"]?.InnerText, out double keepAlive))
                config.KeepAliveInterval = keepAlive;

            if (double.TryParse(gmSettingsNode.Attributes["RetryRegisterGameInterval"]?.InnerText, out double registerGameInterval))
                config.RetryRegisterGameInterval = registerGameInterval;
        }

        //private void LoadGameDefinition(GameConfiguration config, XmlDocument doc)
        //{
        //    XmlNode gameDefinitionNode = doc.DocumentElement.SelectSingleNode("/GameDefinition");

        //    config.Goals = new List<MockGoal>();
        //}
    }
}
