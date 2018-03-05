using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Shared
{
    public class ConfigurationLoader
    {
        public GameConfiguration LoadConfiguration(XmlDocument doc)
        {
            GameConfiguration result = new GameConfiguration();

            XmlNode gmSettingsNode = doc.DocumentElement;

            if (gmSettingsNode != null)
            {
                LoadGMAttributes(result, gmSettingsNode);
                LoadGameDefinition(result, gmSettingsNode);
            }

            return result;
        }

        public GameConfiguration LoadConfiguration(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            return LoadConfiguration(doc);
        }

        private void LoadGMAttributes(GameConfiguration config, XmlNode gmSettingsNode)
        {
            if (double.TryParse(gmSettingsNode.Attributes["KeepAliveInterval"]?.InnerText, out double keepAlive))
                config.KeepAliveInterval = keepAlive;

            if (double.TryParse(gmSettingsNode.Attributes["RetryRegisterGameInterval"]?.InnerText, out double registerGameInterval))
                config.RetryRegisterGameInterval = registerGameInterval;
        }

        private void LoadGameDefinition(GameConfiguration config, XmlNode gmSettingsNode)
        {
            XmlNode gameDefinitionNode = gmSettingsNode["GameDefinition"];

            if (gameDefinitionNode != null)
            {
                config.Goals =  GetGameGoals(gameDefinitionNode);
            }
        }

        private List<MockGoal> GetGameGoals(XmlNode gameDefinitionNode)
        {
             var result = new List<MockGoal>();

            foreach (XmlNode node in gameDefinitionNode.ChildNodes)
            {
                if (node.Name != "Goals")
                    continue;

                var goal = new MockGoal();

                if (!int.TryParse(node.Attributes["x"]?.InnerText, out int x))
                    continue;

                if (!int.TryParse(node.Attributes["y"]?.InnerText, out int y))
                    continue;

                goal.Team = node.Attributes["team"].InnerText;
                goal.X = x;
                goal.Y = y;
                goal.Type = node.Attributes["type"].InnerText;

                result.Add(goal);
            }

            return result;
        }
    }
}
