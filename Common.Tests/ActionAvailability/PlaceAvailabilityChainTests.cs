using System.Collections.Generic;
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

            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidSuccessPlace, pieceId);
        }

        private readonly int boardWidth = 5;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly int pieceId = 1;
        private readonly string playerGuidSuccessPlace = "c094cab7-da7b-457f-89e5-a5c51756035f";
        private readonly string playerGuidFailPlace = "c094cab7-da7b-457f-89e5-a5c51756035d";
        private readonly Dictionary<string, int> playerGuidToPieceId;
        private readonly Location locationFail;
        private readonly Location locationSuccess;
        private Location goalAreaLocation;
        private readonly MockBoard board;


        [Fact]
        public void PlacingPieceOnNonEmptyField()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationFail, board, playerGuidSuccessPlace, playerGuidToPieceId);
            Assert.False(placeAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerPlaceWhenCarryingNoPiece()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationSuccess, board, playerGuidFailPlace, playerGuidToPieceId);
            Assert.False(placeAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PlayerPlaceWhenCarryingPiece()
        {
            var placeAvailabilityChain =
                new PlaceAvailabilityChain(locationSuccess, board, playerGuidSuccessPlace, playerGuidToPieceId);
            Assert.True(placeAvailabilityChain.ActionAvailable());
        }
    }
}