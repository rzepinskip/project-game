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
        public int _playerCount = 8;

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
            Players = GeneratePlayers();
            GameMaster = GenerateGameMaster(Players);

            GenerateThreads(GameMaster, Players);
            RunThreads();
        }

        private List<Player.Player> GeneratePlayers()
        {
            var players = new List<Player.Player>(_playerCount);

            for (int i = 0; i < _playerCount; i++)
            {
                var player = new Player.Player(GenerateBasicBoard())
                {
                    Id = i,
                    RequestsQueue = new Queue<GameMessage>(),
                    ResponsesQueue = new Queue<ResponseMessage>(),
                    Team = i % 2 == 0 ? CommonResources.TeamColour.Red : CommonResources.TeamColour.Blue

                };
                players.Add(player);
            }

            return players;
        }
        private Board GenerateBasicBoard()
        {
            return new Board(10, 10, 5);
        }
        private Board GenerateFullBoard(List<Player.Player> players)
        {
            var board = GenerateBasicBoard();

            for (int i = 0; i < players.Count; i++)
            {
                var player = Players[i];

                var location = new Location();
                if (player.Team == CommonResources.TeamColour.Blue)
                {
                    location = new Location(i, board.GoalAreaSize);
                }
                else
                {
                    location = new Location(i, board.Height - (board.GoalAreaSize + 1));
                }

                var playerInfo = new PlayerInfo
                {
                    Location = location,
                    Team = player.Team,
                    Role = PlayerBase.PlayerType.Member,
                };
                board.Players.Add(player.Id, playerInfo);
            }

            int pieceId = 1;
            var pieceLocation = new Location() { X = 0, Y = 10 };
            board.PlacePieceInTaskArea(pieceId, pieceLocation);
            board.Pieces.Add(pieceId, new Piece() { Id = pieceId });

            return board;
        }
        private GameMaster.GameMaster GenerateGameMaster(List<Player.Player> players)
        {
            var gameMaster = new GameMaster.GameMaster(GenerateFullBoard(players));

            foreach (var player in players)
            {
                gameMaster.RequestsQueues.Add(player.RequestsQueue);
                gameMaster.ResponsesQueues.Add(player.ResponsesQueue);
            }

            return gameMaster;
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
