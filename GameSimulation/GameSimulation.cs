using System.Collections.Generic;
using BoardGenerators.Loaders;
using Common;
using CommunicationServer;
using GameMaster;
using GameMaster.Configuration;

namespace GameSimulation
{
    internal class GameSimulation
    {
        public GameCommunicationServerCommunicator CommunicationServerCommunicator;

        public GameSimulation(string configFilePath)
        {
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);
            CommunicationServerCommunicator = new GameCommunicationServerCommunicator();

            GameMaster = new GameMaster.GameMaster(config);
            Players = new List<Player.Player>();
            for (var i = 0; i < 2 * config.GameDefinition.NumberOfPlayersPerTeam; i++)
            {
                var player = new Player.Player();
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