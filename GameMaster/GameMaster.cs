using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using CsvHelper;

namespace GameMaster
{
    public class GameMaster
    {
        Dictionary<string, int> PlayerGuidToId { get; }
        Board Board { get; set; }

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
