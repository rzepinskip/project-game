using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class ActionLog
    {
        public string ActionType { get; set; }
        public DateTime Timestamp { get; set; }
        public int PlayerID { get; set; }
        public CommonResources.TeamColour Colour { get; set; }
        public PlayerBase.PlayerType Type { get; set; }

    }
}
