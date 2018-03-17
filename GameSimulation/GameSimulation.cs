using Player;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace GameSimulation
{
    class GameSimulation
    {
        public int _iterations;
        public int _minInterval = 500;
        public int _maxInterval = 2000;

        private Random rd = new Random();

        private Thread _gameMasterThread;
        private List<Thread> _playerThreads = new List<Thread>();

        public GameMaster.GameMaster GameMaster { get; set; }
        public List<Player.Player> Players { get; set; }

        public GameSimulation(int iterations)
        {
            _iterations = iterations;
        }
        public void StartSimulation()
        {
            GameMaster = GenerateGameMaster();
            Players = GeneratePlayers(GameMaster);

            CreateQueues(GameMaster, Players);

            GenerateThreads(GameMaster, Players);
            RunThreads();
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

        private GameMaster.GameMaster GenerateGameMaster()
        {
            var gameMaster = new GameMaster.GameMaster();
            gameMaster.PrepareBoard();

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
                player.InitializePlayer(i, playerInfo.Team, playerInfo.Role ,playerBoard, playerInfo.Location);
                players.Add(player);
            }

            return players;
        }

        private void GenerateThreads(GameMaster.GameMaster gameMaster, List<Player.Player> players)
        {
            _gameMasterThread = new Thread(() => GameMasterGameplay(gameMaster));

            foreach (var player in players)
            {
                _playerThreads.Add(new Thread(() => PlayerGameplay(player)));
            }
        }
        private void RunThreads()
        {
            _gameMasterThread.Start();

            foreach (var playerThread in _playerThreads)
            {
                playerThread.Start();
            }
        }

        public void PlayerGameplay(Player.Player player)
        {
            for (int i = 0; i < _iterations; i++)
            {
                Thread.Sleep(rd.Next(_minInterval, _maxInterval));
                var message = new Move()
                {
                    PlayerId = player.Id,
                    Direction = player.Team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up,
                };
                player.RequestsQueue.Enqueue(message);
            }
        }
        public void GameMasterGameplay(GameMaster.GameMaster gameMaster)
        {
            for (int i = 0; i < _iterations; i++)
            {
                Thread.Sleep(rd.Next(_minInterval, _maxInterval));
                foreach (var queue in gameMaster.RequestsQueues)
                {
                    if (queue.Count > 0)
                    {
                        var request = queue.Dequeue();
                        var requesterInfo = gameMaster.Board.Players[request.PlayerId];
                        var response = request.Execute(gameMaster.Board);
                        gameMaster.ResponsesQueues[request.PlayerId].Enqueue(response);
                    }
                }
            }
        }

    }
}
