using GameMaster;
using GameMaster.Configuration;
using Player;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameSimulation
{
    class GameSimulation
    {
        public GameMaster.GameMaster GameMaster { get; private set; }
        public List<Player.Player> Players { get; private set; }
        public PieceGenerator PieceGenerator { get; private set; }

        private int _minInterval = 5;
        private int _maxInterval = 20;
        private int _spawnPieceFrequency;
        private Random _random = new Random();

        public bool GameFinished { get; private set; } = false;

        private Thread _gameMasterThread;
        private Thread _pieceGeneratorThread;
        private List<Thread> _playerThreads = new List<Thread>();

        public GameSimulation(string configFilePath)
        {
            var configLoader = new ConfigurationLoader();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);

            _spawnPieceFrequency = Convert.ToInt32(config.GameDefinition.PlacingNewPiecesFrequency);

            GameMaster = GenerateGameMaster(config);
            PieceGenerator = GameMaster.CreatePieceGenerator(GameMaster.Board);
            Players = GeneratePlayers(GameMaster);

            CreateQueues(GameMaster, Players);
            GenerateThreads(GameMaster, Players, PieceGenerator);
        }

        private void CreateQueues(GameMaster.GameMaster gameMaster, List<Player.Player> players)
        {
            foreach (var player in players)
            {
                player.RequestsQueue = new Queue<GameMessage>();
                player.ResponsesQueue = new Queue<ResponseMessage>();

                gameMaster.RequestsQueues.Add(player.RequestsQueue);
                gameMaster.ResponsesQueues.Add(player.ResponsesQueue);
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

            for (int i = 0; i < playersCount; i++)
            {
                var playerBoard = new Board(gameMaster.Board.Width, gameMaster.Board.TaskAreaSize, gameMaster.Board.GoalAreaSize);
                var playerInfo = gameMaster.Board.Players[i];
                var player = new Player.Player();
                player.InitializePlayer(i, playerInfo.Team, playerInfo.Role, playerBoard, playerInfo.Location);
                players.Add(player);
            }

            return players;
        }

        private void GenerateThreads(GameMaster.GameMaster gameMaster, List<Player.Player> players, PieceGenerator pieceGenerator)
        {
            _gameMasterThread = new Thread(() => GameMasterGameplay(gameMaster));

            _pieceGeneratorThread = new Thread(() => PieceGeneratorGameplay(pieceGenerator));

            foreach (var player in players)
            {
                _playerThreads.Add(new Thread(() => PlayerGameplay(player)));
            }
        }
        public void StartSimulation()
        {
            _gameMasterThread.Start();
            _pieceGeneratorThread.Start();

            foreach (var playerThread in _playerThreads)
            {
                playerThread.Start();
            }
        }

        private void PlayerGameplay(Player.Player player)
        {
            //var initMessage = new Move()
            //{
            //    PlayerId = player.Id,
            //    Direction = player.Team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up,
            //};
            //player.RequestsQueue.Enqueue(initMessage);
            player.RequestsQueue.Enqueue(player.GetNextRequestMessage());
            while (!GameFinished)
            {

                Thread.Sleep(_random.Next(_minInterval, _maxInterval));
                if (player.ResponsesQueue.Count != 0)
                {
                    player.UpdateBoard(player.ResponsesQueue.Dequeue());
                    //
                    //change board state based on response 
                    //  - update method in Response Message
                    //based on board state change strategy state
                    //  - implement strategy
                    //  - hold current state
                    //  - implement state changing action (stateless in next iteration) which return new message
                    //
                    //var message = new Move()
                    //{
                    //    PlayerId = player.Id,
                    //    Direction = player.Team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up,
                    //};
                    player.RequestsQueue.Enqueue(player.GetNextRequestMessage());

                }
            }
        }
        private void GameMasterGameplay(GameMaster.GameMaster gameMaster)
        {
            while (!gameMaster.CheckGameEndCondition())
            {
                Thread.Sleep(_random.Next(_minInterval, _maxInterval));
                foreach (var queue in gameMaster.RequestsQueues)
                {
                    if (queue.Count > 0)
                    {
                        SendResponse(gameMaster, queue.Dequeue());
                    }
                }
            }

            GameFinished = true;
        }

        private async Task SendResponse(GameMaster.GameMaster gameMaster, GameMessage request)
        {
            var delay = Convert.ToInt32(request.GetDelay(gameMaster.GameConfiguration.ActionCosts));
            await Task.Delay(delay);

            var requesterInfo = gameMaster.Board.Players[request.PlayerId];
            var response = request.Execute(gameMaster.Board);
            gameMaster.ResponsesQueues[request.PlayerId].Enqueue(response);
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
