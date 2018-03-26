using System.Collections.Generic;
using System.Xml.Serialization;
using Common.BoardObjects;
using GameMaster.Configuration;

namespace GameMaster.Tests.BoardConfigurationGenerator
{
    class GameDefinitionExtended
    {
        [XmlElement] public List<Location> PiecesLocations;
        [XmlElement] public List<Location> RedPlayersLocations;
        [XmlElement] public List<Location> BluePlayersLocations;
        [XmlElement] public List<GoalField> Goals { get; set; }

    }
}
