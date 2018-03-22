using System;
using System.Collections.Generic;
using System.Threading;
using Common;
using Common.Interfaces;
using GameMaster;
using GameMaster.Configuration;
using Messaging;
using Messaging.Requests;
using Messaging.Responses;
using Player;

namespace GameSimulation
{
    internal class GameSimulation
    {
        private readonly Thread _pieceGeneratorThread;

        private readonly int _spawnPieceFrequency;
        private Random _random = new Random();

        public GameSimulation(string configFilePath)
        {
            var configLoader = new ConfigurationLoader();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);

            _spawnPieceFrequency = Convert.ToInt32(config.GameDefinition.PlacingNewPiecesFrequency);

            GameMaster = GenerateGameMaster(config);
            PieceGenerator = GameMaster.CreatePieceGenerator(GameMaster.Board);
            Players = GeneratePlayers(GameMaster);

            GameMaster.GameFinished += GameMaster_GameFinished;

            CreateQueues(GameMaster, Players);
            _pieceGeneratorThread = new Thread(() => PieceGeneratorGameplay(PieceGenerator));
        }

        public GameMaster.GameMaster GameMaster { get; }
        public List<Player.Player> Players { get; }
        public PieceGenerator PieceGenerator { get; }

        public bool GameFinished { get; private set; }
        public TeamColor Winners { get; private set; }

        private void GameMaster_GameFinished(object sender, GameFinishedEventArgs e)
        {
            //should wait for all threads end
            Winners = e.Winners;
            GameFinished = true;
        }

        private void CreateQueues(GameMaster.GameMaster gameMaster, List<Player.Player> players)
        {
            foreach (var player in players)
            {
                player.RequestsQueue = new ObservableQueue<Request>();
                player.ResponsesQueue = new ObservableQueue<Response>();

                gameMaster.RequestsQueues.Add(player.Id, player.RequestsQueue);
                gameMaster.ResponsesQueues.Add(player.Id, player.ResponsesQueue);
            }
        }

        private GameMaster.GameMaster GenerateGameMaster(GameConfiguration config)
        {
            var gameMaster = new GameMaster.GameMaster(config);

            return gameMaster;
        }

        private List<Player.Player> GeneratePlayers(GameMaster.GameMaster gameMaster)
        {
            var playersCount = gameMaster.Board.Players.Count;
            var players = new List<Player.Player>(playersCount);

            for (var i = 0; i < playersCount; i++)
            {
                var playerBoard = new PlayerBoard(gameMaster.Board.Width, gameMaster.Board.TaskAreaSize,
                    gameMaster.Board.GoalAreaSize);
                var playerInfo = gameMaster.Board.Players[i];
                var player = new Player.Player();
                player.InitializePlayer(i, playerInfo.Team, playerInfo.Role, playerBoard, playerInfo.Location);
                players.Add(player);
            }

            return players;
        }

        public void StartSimulation()
        {
            _pieceGeneratorThread.Start();

            GameMaster.StartListeningToRequests();

            foreach (var player in Players)
            {
                player.StartListeningToResponses();
                player.RequestsQueue.Enqueue(player.GetNextRequestMessage());
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