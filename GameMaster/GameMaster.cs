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

namespace GameMaster
{
    public class GameMaster
    {
        public List<Queue<GameMessage>> RequestsQueues { get; set; } = new List<Queue<GameMessage>>();
        public List<Queue<ResponseMessage>> ResponsesQueues { get; set; } = new List<Queue<ResponseMessage>>();
        public Board Board { get; set; }
        Dictionary<string, int> PlayerGuidToId { get; }

        public GameConfiguration GameConfiguration { get; set; }

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
    }
}
