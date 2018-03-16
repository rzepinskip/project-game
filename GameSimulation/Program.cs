using System;
using Player;
using Shared.BoardObjects;
using System.Collections;
using Shared.GameMessages;
using System.Collections.Generic;
using System.Threading;
using Shared.ResponseMessages;
using Shared;

namespace GameSimulation
{
    class Program
    {
        private static int iterations = 10000;
        private static int minInterval = 1000;
        private static int maxInterval = 2000;
        private static Random rd = new Random();

        static void Main(string[] args)
        {
            var players = GeneratePlayers();
            var gm = new GameMaster.GameMaster(GenerateBoard(players));

            foreach (var player in players)
            {
                gm.RequestsQueues.Add(player.RequestsQueue);
                gm.ResponsesQueues.Add(player.ResponsesQueue);

                var playerThread = new Thread(() => PlayerGameplay(player));
                playerThread.Start();
            }

            var gameMasterThread = new Thread(() => GameMasterGameplay(gm));
            gameMasterThread.Start();

            var boardVisualizer = new BoardVisualizer();
            for (int i = 0; i < iterations; i++)
            {
                Thread.Sleep(1000);
                boardVisualizer.VisualizeBoard(gm.Board);
                Console.WriteLine(i);
            }
        }

        private static void PlayerGameplay(Player.Player player)
        {
            for (int i = 0; i < iterations; i++)
            {
                Thread.Sleep(rd.Next(minInterval, maxInterval));

                if (player.ResponsesQueue.Count > 0)
                {
                    //Console.WriteLine("P: received");
                }

                var message = new Move()
                {
                    PlayerId = player.Id,
                    Direction = player.Team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up,
                };
                player.RequestsQueue.Enqueue(message);

                // Console.WriteLine("P:" + message.ToLog(player.Id, new PlayerInfo()));
            }
        }
        private static void GameMasterGameplay(GameMaster.GameMaster gameMaster)
        {
            for (int i = 0; i < iterations; i++)
            {
                Thread.Sleep(rd.Next(minInterval, maxInterval));
                foreach (var queue in gameMaster.RequestsQueues)
                {
                    if (queue.Count > 0)
                    {
                        var request = queue.Dequeue();
                        var requesterInfo = gameMaster.Board.Players[request.PlayerId];
                        // Console.WriteLine("GM:" + request.ToLog(request.PlayerId, requesterInfo));
                        var response = request.Execute(gameMaster.Board);
                        gameMaster.ResponsesQueues[request.PlayerId].Enqueue(response);
                    }
                }
            }
        }

        private static List<Player.Player> GeneratePlayers()
        {
            var playersCount = 2;
            var players = new List<Player.Player>();

            for (int i = 0; i < playersCount; i++)
            {
                var player = new Player.Player(new Board(10, 10, 5))
                {
                    Id = i,
                    RequestsQueue = new Queue<GameMessage>(),
                    ResponsesQueue = new Queue<ResponseMessage>(),
                };

                players.Add(player);
            }

            return players;
        }
        private static Board GenerateBoard(List<Player.Player> players)
        {
            var board = new Board(10, 10, 5);
            var count = 0;

            foreach (var player in players)
            {
                player.Team = player.Id % 2 == 0 ? CommonResources.TeamColour.Red : CommonResources.TeamColour.Blue;

                var location = new Location();
                if (player.Team == CommonResources.TeamColour.Blue)
                {
                    location = new Location(count / 2, board.GoalAreaSize);
                }
                else
                {
                    location = new Location(count / 2, board.Height - (board.GoalAreaSize + 1));
                }
                count++;

                var playerInfo = new PlayerInfo
                {
                    Location = location,
                    Team = player.Team,
                    Role = PlayerBase.PlayerType.Member,
                };

                board.Players.Add(player.Id, playerInfo);
            }


            int pieceId = 1;
            var pieceLocation = new Location() { X = 2, Y = 3 };
            board.PlacePieceInTaskArea(pieceId, pieceLocation);

            return board;
        }
    }
}
