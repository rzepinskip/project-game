using System.Collections.Generic;
using System.Xml.Serialization;
using BoardGenerators.Loaders;

namespace TestScenarios.DeterministicGame
{
    [XmlRoot(ElementName = "DeterministicGameDefinition", Namespace = "https://se2.mini.pw.edu.pl/17-pl-19/17-pl-19/")]
    public class DeterministicGameDefinition : GameDefinitionBase
    {
        public List<PieceWithLocation> Pieces { get; set; }
        public List<PlayerWithLocation> Players { get; set; }
    }
}