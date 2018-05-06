﻿using System;
using System.Collections.Generic;
using System.Net;
using BoardGenerators.Loaders;
using Common;
using Communication.Client;
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
            var port = Constants.DefaultPortNumber;
            var ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            var ipAddress = ipHostInfo.AddressList[0];

            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);
            var communicationClient = new AsynchronousClient(new TcpSocketConnector(MessageSerializer.Instance, port, ipAddress,
                TimeSpan.FromMilliseconds((int) config.KeepAliveInterval)));

            CommunicationServer = new CommunicationServer.CommunicationServer(MessageSerializer.Instance, config.KeepAliveInterval, port);
            GameMaster = new GameMaster.GameMaster(config, communicationClient);
            Players = new List<Player.Player>();
            for (var i = 0; i < 2 * config.GameDefinition.NumberOfPlayersPerTeam; i++)
            {
                communicationClient = new AsynchronousClient(new TcpSocketConnector(MessageSerializer.Instance, port, ipAddress,
                    TimeSpan.FromMilliseconds((int) config.KeepAliveInterval)));
                var player = new Player.Player(communicationClient, "game", TeamColor.Blue, PlayerType.Leader);
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