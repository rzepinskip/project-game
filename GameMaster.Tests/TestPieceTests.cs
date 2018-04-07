using BoardGenerators.Loaders;
using TestScenarios.DeterministicGame;
using Xunit;

namespace GameMaster.Tests
{
    public class TestPieceTests
    {

        public TestPieceTests()
        {
            var bcl = new XmlLoader<DeterministicGameDefinition>();
            var bc = bcl.LoadConfigurationFromFile("Resources/SimpleBoardConfiguration.xml");
            //var board = new DeterministicGameMasterBoardGenerator().InitializeBoard(bc);
        }
        [Fact]
        public void TestShamPiece()
        {

        }

        [Fact]
        public void TestValidPiece()
        {

        }
    }
}
