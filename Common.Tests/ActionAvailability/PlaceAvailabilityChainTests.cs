using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;
using Xunit;

namespace Common.Tests.ActionAvailability
{
    public class PlaceAvailabilityChainTests
    {
        public PlaceAvailabilityChainTests()
        {
            board = new MockBoard(boardWidth, taskAreaSize, goalAreaSize)
            {
                [new Location(1, 3)] = {PlayerId = 1},
                [new Location(3, 3)] = {PlayerId = 2},
                [new Location(2, 4)] = {PlayerId = 3},
                [new Location(2, 2)] = {PlayerId = 4}
            };
            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 3);
            goalAreaLocation = new Location(1, 1);

            board.PlacePieceInTaskArea(1, locationFail);

            board.Players.Add(playerIdSuccess,
                new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationSuccess, new Piece(0, PieceType.Normal)));
            board.Players.Add(playerIdFail, new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationFail));
        }

        private readonly int boardWidth = 5;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly int pieceId = 1;
        private readonly int playerIdSuccess = 0;
        private readonly int playerIdFail = 1;
        private readonly Location locationFail;
        private readonly Location locationSuccess;
        private readonly Location goalAreaLocation;
        private readonly MockBoard board;


        [Fact]
        public void PlacingPieceOnNonEmptyField()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationFail, board, playerIdSuccess);
            Assert.False(placeAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerPlaceWhenCarryingNoPiece()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationSuccess, board, playerIdFail);
            Assert.False(placeAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerPlaceWhenCarryingPiece()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationSuccess, board, playerIdSuccess);
            Assert.True(placeAvailabilityChain.ActionAvailable());
        }
    }
}