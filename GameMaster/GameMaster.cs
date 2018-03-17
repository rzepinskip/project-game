using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using CsvHelper;
using Shared.ResponseMessages;

namespace GameMaster
{
    public class GameMaster
    {
        public List<Queue<GameMessage>> RequestsQueues { get; set; } = new List<Queue<GameMessage>>();
        public List<Queue<ResponseMessage>> ResponsesQueues { get; set; } = new List<Queue<ResponseMessage>>();
        public Board Board { get; set; }

        Dictionary<string, int> PlayerGuidToId { get; }
        
        public GameMaster()
        { }
        public GameMaster(Board board)
        {
            Board = board;
        }

        public void PutLog(string filename, ActionLog log)
        {
            using (var textWriter = new StreamWriter(filename, true))
                using (var csvWriter = new CsvWriter(textWriter))
                {
                csvWriter.NextRecord();
                csvWriter.WriteRecord(log);
                }
        }
    }
}
