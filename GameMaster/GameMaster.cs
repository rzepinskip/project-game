using System.Collections.Generic;
using System.IO;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using CsvHelper;
using Shared.ResponseMessages;
using GameMaster.Configuration;

namespace GameMaster
{
    public class GameMaster
    {
        public List<Queue<GameMessage>> RequestsQueues { get; set; } = new List<Queue<GameMessage>>();
        public List<Queue<ResponseMessage>> ResponsesQueues { get; set; } = new List<Queue<ResponseMessage>>();
        public Board Board { get; set; }
        Dictionary<string, int> PlayerGuidToId { get; }

        public void PrepareBoard(string configFilePath)
        {
            var configLoader = new ConfigurationLoader();
            var config = configLoader.LoadConfigurationFromFile(configFilePath);
            var gameDefiniton = config.GameDefinition;

            var boardGenerator = new BoardGenerator();
            Board = boardGenerator.InitializeBoard(gameDefiniton);
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
    }
}
