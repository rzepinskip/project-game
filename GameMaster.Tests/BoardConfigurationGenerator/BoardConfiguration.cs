using System.Collections.Generic;
using System.Xml.Serialization;
using Common.BoardObjects;
using GameMaster.Configuration;

namespace GameMaster.Tests.BoardConfigurationGenerator
{
    [XmlRoot(ElementName = "BoardConfiguration", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class BoardConfiguration : GameDefinitionBase
    {
        [XmlElement] public List<Location> PiecesLocations { get; set; }
        [XmlElement] public List<Location> RedPlayersLocations { get; set; }
        [XmlElement] public List<Location> BluePlayersLocations { get; set; }
    }
}
