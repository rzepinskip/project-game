using System;
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
        private readonly Thread _pieceGeneratorThread;

        private readonly int _spawnPieceFrequency;

        public GameSimulation(string configFilePath)
        {
            var configLoader = new XmlLoader<GameConfiguration>();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);

            _spawnPieceFrequency = Convert.ToInt32(config.GameDefinition.PlacingNewPiecesFrequency);

            _communicationServer = new GameCommunicationServer();
            
            GameMaster = GenerateGameMaster(config);
            PieceGenerator = GameMaster.CreatePieceGenerator(GameMaster.Board);
            Players = GeneratePlayers(GameMaster).Result;

            GameMaster.GameFinished += GameMaster_GameFinished;

            CreateQueues(GameMaster, Players);
            _pieceGeneratorThread = new Thread(() => PieceGeneratorGameplay(PieceGenerator));
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
            _pieceGeneratorThread.Join();
        }

        private void CreateQueues(GameMaster.GameMaster gameMaster, List<Player.Player> players)
        {
            foreach (var player in players)
            {
                player.RequestsQueue = new ObservableConcurrentQueue<IRequest>();
                player.ResponsesQueue = new ObservableConcurrentQueue<IMessage>();

                gameMaster.RequestsQueues.Add(player.Id, player.RequestsQueue);
                gameMaster.ResponsesQueues.Add(player.Id, player.ResponsesQueue);
                gameMaster.IsPlayerQueueProcessed.Add(player.Id, false);
                gameMaster.IsPlayerQueueProcessedLock.Add(player.Id, new object());
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
                var playerBoard = new PlayerBoard(gameMaster.Board.Width, gameMaster.Board.TaskAreaSize,
                    gameMaster.Board.GoalAreaSize);
                var playerInfo = gameMaster.Board.Players[i];
                var player = new Player.Player();

                string playerGuid = "";
                foreach (var guidIdPair in GameMaster.PlayerGuidToId)
                {
                    if (guidIdPair.Value == i)
                    {
                        playerGuid = guidIdPair.Key;
                        break;
                    }
                }

                await player.InitializePlayer(i, playerGuid, -1, playerInfo.Team, playerInfo.Role, playerBoard, playerInfo.Location);
                players.Add(player);
            }

            Debug.WriteLine("finished player intialization");
            //Thread.Sleep(10000);
            return players;
        }

        public void StartSimulation()
        {
            _pieceGeneratorThread.Start();

            GameMaster.StartListeningToRequests();

            foreach (var player in Players)
            {
                //player.StartListeningToResponses();
                //player.RequestsQueue.Enqueue(player.GetNextRequestMessage());
                player.CommunicationClient.Send(player.GetNextRequestMessage());
            }
        }

        private void PieceGeneratorGameplay(PieceGenerator pieceGenerator)
        {
            while (!GameFinished)
            {
                Thread.Sleep(_spawnPieceFrequency);
                pieceGenerator.SpawnPiece();
            }
        }
    }
}