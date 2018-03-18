using System.Collections.Generic;
using System.IO;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using CsvHelper;
using Shared.ResponseMessages;
using GameMaster.Configuration;
using System;
using System.Linq;
using System.Threading;

namespace GameMaster
{
    public class GameMaster
    {
        public Dictionary<int, ObservableQueue<GameMessage>> RequestsQueues { get; set; } = new Dictionary<int, ObservableQueue<GameMessage>>();
        public Dictionary<int, ObservableQueue<ResponseMessage>> ResponsesQueues { get; set; } = new Dictionary<int, ObservableQueue<ResponseMessage>>();
        public Board Board { get; set; }
        public GameConfiguration GameConfiguration { get; private set; }
        public bool GameFinished { get; private set; }

        private Dictionary<string, int> PlayerGuidToId { get; }


        public GameMaster(GameConfiguration gameConfiguration)
        {
            GameConfiguration = gameConfiguration;

            var boardGenerator = new BoardGenerator();
            Board = boardGenerator.InitializeBoard(GameConfiguration.GameDefinition);
        }

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

        public bool CheckGameEndCondition()
        {
            var blueRemainingGoalsCount = 0;
            var redRemainingGoalsCount = 0;

            foreach (var field in Board.Content)
            {
                if (field is GoalField goalField)
                {
                    if (goalField.Type == CommonResources.GoalFieldType.Goal)
                    {
                        if (goalField.Team == CommonResources.TeamColour.Red)
                            redRemainingGoalsCount++;
                        else
                            blueRemainingGoalsCount++;
                    }
                }
            }
            return blueRemainingGoalsCount == 0 || redRemainingGoalsCount == 0;
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

                GameFinished = !CheckGameEndCondition();

                RequestsQueues[request.PlayerId].Dequeue();
            }
        }

        public void ListenToIncomingMessages()
        {
            foreach (var queue in RequestsQueues)
            {
                queue.Value.CollectionChanged += (sender, args) =>
                {
                    new Thread(() => HandleMessagesFromPlayer(queue.Value.Peek().PlayerId)).Start();
                };
            }
        }
    }
}
