﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using BoardGenerators.Loaders;
using Common;
using Common.Interfaces;
using CommunicationServer;
using GameMaster;
using GameMaster.Configuration;
using Player;

namespace GameSimulation
{
    internal class GameSimulation
    {
        private  Thread _pieceGeneratorThread;

        private readonly int _spawnPieceFrequency;

        public GameSimulation(string configFilePath)
        {
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);

            //_spawnPieceFrequency = Convert.ToInt32(config.GameDefinition.PlacingNewPiecesFrequency);

            _communicationServer = new GameCommunicationServer();
            
            GameMaster = GenerateGameMaster(config);
            //PieceGenerator = GameMaster.CreatePieceGenerator(GameMaster.Board);
            Players = GeneratePlayers(GameMaster).Result;

            GameMaster.GameFinished += GameMaster_GameFinished;

            CreateQueues(GameMaster);
            
            
        }

        public GameMaster.GameMaster GameMaster { get; }
        public List<Player.Player> Players { get; }
        public PieceGenerator PieceGenerator { get; }

        public bool GameFinished { get; private set; }
        public TeamColor Winners { get; private set; }

        public GameCommunicationServer _communicationServer;

        private void GameMaster_GameFinished(object sender, GameFinishedEventArgs e)
        {
            Winners = e.Winners;
            GameFinished = true;
            //_pieceGeneratorThread.Join();
        }

        private void CreateQueues(GameMaster.GameMaster gameMaster)
        {
            //foreach (var player in players)
            for(int i = 0; i < gameMaster.Board.Players.Count; ++i)
            {
                var RequestsQueue = new ObservableConcurrentQueue<IRequest>();
                var ResponsesQueue = new ObservableConcurrentQueue<IMessage>();

                gameMaster.RequestsQueues.Add(i, RequestsQueue);
                gameMaster.ResponsesQueues.Add(i, ResponsesQueue);
                gameMaster.IsPlayerQueueProcessed.Add(i, false);
                gameMaster.IsPlayerQueueProcessedLock.Add(i, new object());
            }
        }

        private GameMaster.GameMaster GenerateGameMaster(GameConfiguration config)
        {
            var gameMaster = new GameMaster.GameMaster(config);

            return gameMaster;
        }

        private async Task<List<Player.Player>> GeneratePlayers(GameMaster.GameMaster gameMaster)
        {
            var playersCount = gameMaster.Board.Players.Count;
            var players = new List<Player.Player>(playersCount);

            for (var i = 0; i < playersCount; i++)
            {
                //var playerBoard = new PlayerBoard(gameMaster.Board.Width, gameMaster.Board.TaskAreaSize,
                    //gameMaster.Board.GoalAreaSize);
                //var playerInfo = gameMaster.Board.Players[i];
                var player = new Player.Player();

                //Guid playerGuid;
                //foreach (var guidIdPair in GameMaster.PlayerGuidToId)
                //{
                //    if (guidIdPair.Value == i)
                //    {
                //        playerGuid = guidIdPair.Key;
                //        break;
                //    }
                //}

                player.InitializePlayer(i%2==0 ? TeamColor.Blue:TeamColor.Red);
                players.Add(player);
            }

            return players;
        }

        public void StartSimulation()
        {
            

            //GameMaster.StartListeningToRequests();

            foreach (var player in Players)
            {
                //player.StartListeningToResponses();
                //player.RequestsQueue.Enqueue(player.GetNextRequestMessage());
                player.CommunicationClient.Send(player.GetNextRequestMessage());
            }
        }

        
    }
}