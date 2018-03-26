using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;
using Xunit;

namespace Common.Tests.ActionAvailability
{
    public class TestAvailabilityChainTests
    {
        public TestAvailabilityChainTests()
        {
            _board = new MockBoard(boardWidth, taskAreaSize, goalAreaSize)
            {
                [new Location(1, 3)] = {PlayerId = 1},
                [new Location(3, 3)] = {PlayerId = 2},
                [new Location(2, 4)] = {PlayerId = 3},
                [new Location(2, 2)] = {PlayerId = 4}
            };

            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 3);

            _board.Players.Add(playerIdFail,
                new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationFail, new Piece(0, PieceType.Normal)));
            _board.Players.Add(playerIdSuccess, new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationSuccess));
        }

        private readonly int pieceId = 1;
        private readonly MockBoard _board;
        private readonly int boardWidth = 5;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly int playerIdSuccess = 1;
        private readonly int playerIdFail = 0;
        private readonly Location locationFail;
        private readonly Location locationSuccess;

        [Fact]
        public void PlayerTestWhenCarryingNoPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerIdSuccess, _board.Players);
            Assert.False(testAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerTestWhenCarryingPiece()
        {
            var testAvailabilityChain = new TestAvailabilityChain(playerIdFail, _board.Players);
            Assert.True(testAvailabilityChain.ActionAvailable());
        }
    }
}