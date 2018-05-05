using System.Collections.Generic;
using BoardGenerators.Loaders;
using Common;
using GameMaster;
using GameMaster.Configuration;
using Messaging.Serialization;

namespace GameSimulation
{
    internal class GameSimulation
    {
        public CommunicationServer.CommunicationServer CommunicationServer;

        public GameSimulation(string configFilePath)
        {
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);
            CommunicationServer = new CommunicationServer.CommunicationServer(MessageSerializer.Instance, config.KeepAliveInterval, 11000);

            GameMaster = new GameMaster.GameMaster(config, MessageSerializer.Instance);
            Players = new List<Player.Player>();
            for (var i = 0; i < 2 * config.GameDefinition.NumberOfPlayersPerTeam; i++)
            {
                var player = new Player.Player(MessageSerializer.Instance);
                player.InitializePlayer("game", TeamColor.Blue, PlayerType.Leader);
                Players.Add(player);
            }

            GameMaster.GameFinished += GameMaster_GameFinished;
        }

        public GameMaster.GameMaster GameMaster { get; }
        public List<Player.Player> Players { get; }

        public bool GameFinished { get; set; }
        public TeamColor Winners { get; private set; }

        private void GameMaster_GameFinished(object sender, GameFinishedEventArgs e)
        {
            Winners = e.Winners;
            GameFinished = true;
        }

        public void StartSimulation()
        {
            foreach (var player in Players) player.CommunicationClient.Send(player.GetNextRequestMessage());
        }
    }
}