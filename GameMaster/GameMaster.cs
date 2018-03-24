using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using CsvHelper;
using GameMaster.Configuration;
using Messaging;
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

        public Dictionary<int, ObservableQueue<Request>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableQueue<Request>>();

        public Dictionary<int, ObservableQueue<Response>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableQueue<Response>>();

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

        public PieceGenerator CreatePieceGenerator(IBoard board)
        {
            return new PieceGenerator(board, GameConfiguration.GameDefinition.ShamProbability);
        }

        public bool IsGameFinished()
        {
            var blueRemainingGoalsCount = 0;
            var redRemainingGoalsCount = 0;

            foreach (var field in Board)
                if (field is GoalField goalField)
                    if (goalField.Type == GoalFieldType.Goal)
                        if (goalField.Team == TeamColor.Red)
                            redRemainingGoalsCount++;
                        else
                            blueRemainingGoalsCount++;
            return blueRemainingGoalsCount == 0 || redRemainingGoalsCount == 0;
        }

        public TeamColor CheckWinner()
        {
            var blueRemainingGoalsCount = 0;
            var redRemainingGoalsCount = 0;

            foreach (var field in Board)
                if (field is GoalField goalField)
                    if (goalField.Type == GoalFieldType.Goal)
                        if (goalField.Team == TeamColor.Red)
                            redRemainingGoalsCount++;
                        else
                            blueRemainingGoalsCount++;

            if (blueRemainingGoalsCount == 0)
                return TeamColor.Blue;
            if (redRemainingGoalsCount == 0)
                return TeamColor.Red;
            throw new InvalidOperationException();
        }

        public void HandleMessagesFromPlayer(int playerId)
        {
            var requestQueue = RequestsQueues[playerId];
            while (requestQueue.Count > 0)
            {
                var request = requestQueue.Peek();
                var delay = Convert.ToInt32(request.GetDelay(GameConfiguration.ActionCosts));
                Thread.Sleep(delay);

                var requesterInfo = Board.Players[request.PlayerId];
                var response = request.Execute(Board);
                ResponsesQueues[request.PlayerId].Enqueue(response);

                if (IsGameFinished())
                    GameFinished(this, new GameFinishedEventArgs(CheckWinner()));

                RequestsQueues[request.PlayerId].Dequeue();
            }
        }

        public void StartListeningToRequests()
        {
            foreach (var queue in RequestsQueues)
                queue.Value.CollectionChanged += (sender, args) =>
                {
                    new Thread(() => HandleMessagesFromPlayer(queue.Value.Peek().PlayerId)).Start();
                };
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