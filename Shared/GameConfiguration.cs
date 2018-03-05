using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class GameConfiguration
    {
        //Game Definition
        public List<MockGoal> Goals { get; set; }
        public double ShamProbability { get; set; }
        public double PlacingNewPiecesFrequency { get; set; }
        public int InitialNumberOfPieces { get; set; }
        public int BoardWidth { get; set; }
        public int TaskAreaLength { get; set; }
        public int GoalAreaLength { get; set; }
        public int NumberOfPlayersPerTeam { get; set; }
        public string GameName { get; set; }

        //Actions costs
        public double MoveDelay { get; set; }
        public double DiscoverDelay { get; set; }
        public double TestDelay { get; set; }
        public double PickUpDelay { get; set; }
        public double PlacingDelay { get; set; }
        public double KnowledgeExchangeDelay { get; set; }

        //Game Master
        public double KeepAliveInterval { get; set; }
        public double RetryRegisterGameInterval { get; set; }
    }
}
