using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Shared.Board;

namespace GameMaster.Configuration
{
    [XmlRoot(ElementName = "GameMasterSettings", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class GameConfiguration
    {

        //Game Definition
        [XmlElement("GameDefinition")]
        public GameDefinition GD { get; set; }

        //Actions costs
        [XmlElement("ActionCosts")]
        public ActionCosts AC { get; set; }

        //Game Master
        [XmlAttribute("KeepAliveInterval")]
        public double KeepAliveInterval { get; set; }
        [XmlAttribute("RetryRegisterGameInterval")]
        public double RetryRegisterGameInterval { get; set; }
    }

    [XmlRoot("GameDefinition")]
    public class GameDefinition
    {
        [XmlElement("Goals")]
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

    [XmlRoot("ActionCosts")]
    public class ActionCosts
    {
        public double MoveDelay { get; set; }
        public double DiscoverDelay { get; set; }
        public double TestDelay { get; set; }
        public double PickUpDelay { get; set; }
        public double PlacingDelay { get; set; }
        public double KnowledgeExchangeDelay { get; set; }
    }
}
