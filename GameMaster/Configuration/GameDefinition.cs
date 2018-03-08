using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GameMaster.Configuration
{
    public class GameDefinition
    {
        [XmlElement]
        public List<GoalField> Goals { get; set; }

        public double ShamProbability { get; set; }

        public double PlacingNewPiecesFrequency { get; set; }

        public int InitialNumberOfPieces { get; set; }

        public int BoardWidth { get; set; }

        public int TaskAreaLength { get; set; }

        public int GoalAreaLength { get; set; }

        public int NumberOfPlayersPerTeam { get; set; }

        public string GameName { get; set; }
    }
}
