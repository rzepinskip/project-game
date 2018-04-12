using System.Collections.Generic;
using System.Xml.Serialization;
using Common.BoardObjects;

namespace BoardGenerators.Loaders
{
    public abstract class GameDefinitionBase
    {
        [XmlElement]
        public List<GoalField> Goals { get; set; }

        public int BoardWidth { get; set; }

        public int TaskAreaLength { get; set; }

        public int GoalAreaLength { get; set; }

    }
}
