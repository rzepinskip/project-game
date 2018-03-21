using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Common;
using Common.BoardObjects;
using CsvHelper;
using GameMaster.Configuration;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            var boardGenerator = new BoardGenerator();
            Board = boardGenerator.InitializeBoard(GameConfiguration.GameDefinition);
        }

        public Dictionary<int, ObservableQueue<GameMessage>> RequestsQueues { get; set; } =
            new Dictionary<int, ObservableQueue<GameMessage>>();

        public Dictionary<int, ObservableQueue<ResponseMessage>> ResponsesQueues { get; set; } =
            new Dictionary<int, ObservableQueue<ResponseMessage>>();

        public GameConfiguration GameConfiguration { get; }
        private Dictionary<string, int> PlayerGuidToId { get; }
        public IGameMasterBoard Board { get; set; }

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

        public PieceGenerator CreatePieceGenerator(Board board)
        {
            return new PieceGenerator(board, GameConfiguration.GameDefinition.ShamProbability);
        }

        public bool IsGameFinished()
        {
            var blueRemainingGoalsCount = 0;
            var redRemainingGoalsCount = 0;

            foreach (var field in Board)
                if (field is GoalField goalField)
                    if (goalField.Type == CommonResources.GoalFieldType.Goal)
                        if (goalField.Team == CommonResources.TeamColour.Red)
                            redRemainingGoalsCount++;
                        else
                            blueRemainingGoalsCount++;
            return blueRemainingGoalsCount == 0 || redRemainingGoalsCount == 0;
        }

        public CommonResources.TeamColour CheckWinner()
        {
            var blueRemainingGoalsCount = 0;
            var redRemainingGoalsCount = 0;

            foreach (var field in Board.Content)
                if (field is GoalField goalField)
                    if (goalField.Type == CommonResources.GoalFieldType.Goal)
                        if (goalField.Team == CommonResources.TeamColour.Red)
                            redRemainingGoalsCount++;
                        else
                            blueRemainingGoalsCount++;

            if (blueRemainingGoalsCount == 0)
                return CommonResources.TeamColour.Blue;
            if (redRemainingGoalsCount == 0)
                return CommonResources.TeamColour.Red;
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

        public bool IsDiscoverPossible(string playerGuid)
        {
            throw new NotImplementedException();
        }

        public bool IsMovePossible(string playerGuid, Direction direction)
        {
            throw new NotImplementedException();
        }

        public bool IsPickUpPiecePossible(string playerGuid)
        {
            throw new NotImplementedException();
        }

        public bool IsPlacePiecePossible(string playerGuid)
        {
            throw new NotImplementedException();
        }

        public bool IsTestPiecePossible(string playerGuid)
        {
            throw new NotImplementedException();
        }

        public Common.BoardObjects.Location GetPlayerLocation(string playerGuid)
        {
            throw new NotImplementedException();
        }
    }

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(CommonResources.TeamColour winners)
        {
            Winners = winners;
        }

        public CommonResources.TeamColour Winners { get; set; }
    }
}