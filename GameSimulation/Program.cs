using System;
using Player;
using Shared.BoardObjects;
using System.Collections;
using Shared.GameMessages;
using System.Collections.Generic;
using System.Threading;
using Shared.ResponseMessages;

namespace GameSimulation
{
    class Program
    {
        private static int iterations = 10000;
        private static int minInterval = 3000;
        private static int maxInterval = 4000;
        private static Random rd = new Random();

        private static void PlayerGameplay(Player.Player player)
        {
            for (int i = 0; i < iterations; i++)
            {
                Thread.Sleep(rd.Next(minInterval, maxInterval));

                if (player.ResponsesQueue.Count > 0)
                {
                    Console.WriteLine("P: received");
                }

                var message = new Discover()
                {
                    PlayerId = player.Id
                };
                player.RequestsQueue.Enqueue(message);
                Console.WriteLine("P:" + message.ToLog(player.Id, new Shared.PlayerInfo()));
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
                        Console.WriteLine("GM:" + request.ToLog(request.PlayerId, new Shared.PlayerInfo()));
                        var response = new DiscoverResponse();
                        gameMaster.ResponsesQueues[request.PlayerId].Enqueue(response);
                    }
                }

            }
        }


        static void Main(string[] args)
        {
            var gm = new GameMaster.GameMaster();
            

            var playersCount = 2;
            var players = new List<Player.Player>();

            for (int i = 0; i < playersCount; i++)
            {
                var player = new Player.Player
                {
                    Id = i
                };

                var requestQueue = new Queue<GameMessage>();
                player.RequestsQueue = requestQueue;
                gm.RequestsQueues.Add(requestQueue);

                var responsesQueue = new Queue<ResponseMessage>();
                player.ResponsesQueue = responsesQueue;
                gm.ResponsesQueues.Add(responsesQueue);

                var playerThread = new Thread(() => PlayerGameplay(player));
                playerThread.Start();
            }

            var gameMasterThread = new Thread(() => GameMasterGameplay(gm));
            gameMasterThread.Start();
        }

     
    }
}
