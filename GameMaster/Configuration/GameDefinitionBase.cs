using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace GameMaster.Configuration
{
    public class GameDefinitionBase
    {
        [XmlElement]
        public List<GoalField> Goals { get; set; }

        public int BoardWidth { get; set; }

        public int TaskAreaLength { get; set; }

        public int GoalAreaLength { get; set; }

    }
}
