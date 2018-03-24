using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Common;
using Common.BoardObjects;
using CsvHelper;
using GameMaster.Configuration;
using Messaging.ActionHelpers;
using Messaging.Requests;
using Messaging.Responses;

namespace GameMaster
{
    public class GameMaster
    {
        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            var boardGenerator = new BoardGenerator();
            Board = boardGenerator.InitializeBoard(GameConfiguration.GameDefinition);
        }

        public Dictionary<int, ObservableConcurrentQueue<Request>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<Request>>();

        public Dictionary<int, ObservableConcurrentQueue<Response>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableConcurrentQueue<Response>>();

        public GameConfiguration GameConfiguration { get; }
        private Dictionary<string, int> PlayerGuidToId { get; }
        public GameMasterBoard Board { get; set; }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutLog(string filename, ActionLog log)
        {
            using (var textWriter = new StreamWriter(filename, true))
            {
                using (var csvWriter = new CsvWriter(textWriter))
                {
                    csvWriter.NextRecord();
                    csvWriter.WriteRecord(log);
                }
            }
        }

        public PieceGenerator CreatePieceGenerator(GameMasterBoard board)
        {
            return new PieceGenerator(board, GameConfiguration.GameDefinition.ShamProbability);
        }

        private async void HandleMessagesFromPlayer(int playerId)
        {
            var requestQueue = RequestsQueues[playerId];
            while (requestQueue.Count > 0)
            {
                Request request;
                if (!requestQueue.TryPeek(out request))
                {
                    throw new ConcurrencyException();
                }

                var timeSpan = Convert.ToInt32(request.GetDelay(GameConfiguration.ActionCosts));
                await Task.Delay(timeSpan);

                Response response;
                lock (Board.Lock)
                {
                    response = request.Execute(Board);

                    if (Board.IsGameFinished())
                    {
                        GameFinished(this, new GameFinishedEventArgs(Board.CheckWinner()));
                        response.IsGameFinished = true;
                    }
                }

                ResponsesQueues[request.PlayerId].Enqueue(response);

                if (!RequestsQueues[request.PlayerId].TryDequeue(out var result))
                {
                    throw new ConcurrencyException();
                }
            }
        }

        public void StartListeningToRequests()
        {
            foreach (var queue in RequestsQueues.Values)
            {
                queue.FirstItemEnqueued += (sender, args) =>
                {
                    Task.Run(() => HandleMessagesFromPlayer(args.Item.PlayerId));
                };
            }
        }
    }

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(TeamColor winners)
        {
            Winners = winners;
        }

        public TeamColor Winners { get; set; }
    }
}