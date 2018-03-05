using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace Shared
{
    public class ConfigurationLoader
    {

        public GameConfiguration LoadConfiguration(string filepath)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filepath);

            return LoadConfiguration(doc);
        }

        public GameConfiguration LoadConfiguration(XmlDocument doc)
        {
            GameConfiguration result = new GameConfiguration();

            XmlNode gmSettingsNode = doc.DocumentElement;

            if (gmSettingsNode != null)
            {
                LoadGMAttributes(result, gmSettingsNode);
                LoadGameDefinition(result, gmSettingsNode);
                LoadActionCosts(result, gmSettingsNode);
            }

            return result;
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

                if (double.TryParse(gameDefinitionNode["ShamProbability"]?.InnerText, out double shamProbability))
                    config.ShamProbability = shamProbability;

                if (double.TryParse(gameDefinitionNode["PlacingNewPiecesFrequency"]?.InnerText, out double placingNewPiecesFrequency))
                    config.PlacingNewPiecesFrequency = placingNewPiecesFrequency;

                if (int.TryParse(gameDefinitionNode["InitialNumberOfPieces"]?.InnerText, out int initialNumberOfPieces))
                    config.InitialNumberOfPieces = initialNumberOfPieces;

                if (int.TryParse(gameDefinitionNode["BoardWidth"]?.InnerText, out int boardWidth))
                    config.BoardWidth = boardWidth;

                if (int.TryParse(gameDefinitionNode["TaskAreaLength"]?.InnerText, out int taskAreaLength))
                    config.TaskAreaLength = taskAreaLength;

                if (int.TryParse(gameDefinitionNode["GoalAreaLength"]?.InnerText, out int goalAreaLength))
                    config.GoalAreaLength = goalAreaLength;

                if (int.TryParse(gameDefinitionNode["NumberOfPlayersPerTeam"]?.InnerText, out int numberOfPlayersPerTeam))
                    config.NumberOfPlayersPerTeam = numberOfPlayersPerTeam;

                config.GameName = gameDefinitionNode["GameName"]?.InnerText;
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
        private void LoadActionCosts(GameConfiguration config, XmlNode gmSettingsNode)
        {
            XmlNode actionCostsNode = gmSettingsNode["ActionCosts"];

            if (actionCostsNode != null)
            {
                if (double.TryParse(actionCostsNode["MoveDelay"]?.InnerText, out double moveDelay))
                    config.MoveDelay = moveDelay;

                if (double.TryParse(actionCostsNode["DiscoverDelay"]?.InnerText, out double discoverDelay))
                    config.DiscoverDelay = discoverDelay;

                if (double.TryParse(actionCostsNode["TestDelay"]?.InnerText, out double testDelay))
                    config.TestDelay = testDelay;

                if (double.TryParse(actionCostsNode["PickUpDelay"]?.InnerText, out double pickUpDelay))
                    config.PickUpDelay = pickUpDelay;

                if (double.TryParse(actionCostsNode["PlacingDelay"]?.InnerText, out double placingDelay))
                    config.PlacingDelay = placingDelay;

                if (double.TryParse(actionCostsNode["KnowledgeExchangeDelay"]?.InnerText, out double knowledgeExchangeDelay))
                    config.KnowledgeExchangeDelay = knowledgeExchangeDelay;
            }
        }
    }
}
