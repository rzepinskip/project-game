using System.Collections.Generic;
using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;
using Xunit;

namespace Common.Tests.ActionAvailability
{
    public class PickUpAvailabilityChainTests
    {
        public PickUpAvailabilityChainTests()
        {
            board = new MockBoard(boardWidth, taskAreaSize, goalAreaSize)
            {
                [new Location(1, 3)] = { PlayerId = 1 },
                [new Location(3, 3)] = { PlayerId = 2 },
                [new Location(2, 4)] = { PlayerId = 3 },
                [new Location(2, 2)] = { PlayerId = 4 }
            };
            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 3);
            goalAreaLocation = new Location(1, 1);

            board.PlacePieceInTaskArea(1, locationSuccess);
            
            board.Players.Add(playerIdFail, new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationFail, new Piece(0, PieceType.Normal)));
            board.Players.Add(playerIdSuccess, new PlayerInfo(TeamColor.Blue, PlayerType.Member, locationSuccess));
        }

        private readonly int boardWidth = 5;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly int pieceId = 1;
        private readonly int playerIdSuccess = 1;
        private readonly int playerIdFail = 0;
        private readonly Location locationFail;
        private readonly Location locationSuccess;
        private readonly Location goalAreaLocation;
        private readonly MockBoard board;

        [Fact]
        public void PickUpAvailable()
        {
            var pickUpAvailabilityChain =
                new PickUpAvailabilityChain(locationSuccess, board, playerIdSuccess);
            Assert.True(pickUpAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PickUpOnEmptyTaskField()
        {
            var pickUpAvailabilityChain =
                new PickUpAvailabilityChain(locationFail, board, playerIdSuccess);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PickUpOnGoalArea()
        {
            var pickUpAvailabilityChain =
                new PickUpAvailabilityChain(goalAreaLocation, board, playerIdSuccess);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PickUpWhenPlayerCarringPiece()
        {
            var pickUpAvailabilityChain =
                new PickUpAvailabilityChain(locationSuccess, board, playerIdFail);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }
    }
}