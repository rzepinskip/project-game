using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace GameMaster.Configuration
{
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
