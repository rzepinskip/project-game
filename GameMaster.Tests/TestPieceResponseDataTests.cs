using GameMaster.Configuration;
using GameMaster.Tests.BoardConfigurationGenerator;
using Xunit;

namespace GameMaster.Tests
{
    public class TestPieceResponseDataTests
    {

        public TestPieceResponseDataTests()
        {
            BoardConfigurationLoader bcl = new BoardConfigurationLoader();
            BoardConfiguration bc = bcl.LoadConfigurationFromFile("Resources/SimpleBoardConfiguration.xml");
            var board = new DeterministicBoardGenerator().InitializeBoard(bc);
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
