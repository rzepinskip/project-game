using System.Collections.Generic;
using Xunit;
using Shared.ActionAvailability.AvailabilityChain;

namespace Shared.Tests
{
    public class PickUpAvailabilityChainTests
    {
        private int boardWidth = 5;
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;
        private int pieceId = 1;
        private string playerGuidSuccessPickUp = "c094cab7-da7b-457f-89e5-a5c51756035f";
        private string playerGuidFailPickUp = "c094cab7-da7b-457f-89e5-a5c51756035d";
        private Dictionary<string, int> playerGuidToPieceId;
        private Shared.BoardObjects.Location locationFail;
        private Shared.BoardObjects.Location locationSuccess;
        private Shared.BoardObjects.Location goalAreaLocation;
        private Shared.BoardObjects.Board board;

        public PickUpAvailabilityChainTests()
        {
            board = new Shared.BoardObjects.Board(boardWidth, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.BoardObjects.Location(2, 3);
            locationSuccess = new Shared.BoardObjects.Location(1, 3);
            goalAreaLocation = new Shared.BoardObjects.Location(1, 1);

            board.PlacePieceInTaskArea(1, locationSuccess);

            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidFailPickUp, pieceId);
        }

        [Fact]
        public void PickUpOnEmptyTaskField()
        {
            var pickUpAvailabilityChain = new PickUpAvailabilityChain(locationFail, board, playerGuidSuccessPickUp, playerGuidToPieceId);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }
        
        [Fact]
        public void PickUpOnGoalArea()
        {
            var pickUpAvailabilityChain = new PickUpAvailabilityChain(goalAreaLocation, board, playerGuidSuccessPickUp, playerGuidToPieceId);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }
        [Fact]
        public void PickUpAvailable()
        {
            var pickUpAvailabilityChain = new PickUpAvailabilityChain(locationSuccess, board, playerGuidSuccessPickUp, playerGuidToPieceId);
            Assert.True(pickUpAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void PickUpWhenPlayerCarringPiece()
        {
            var pickUpAvailabilityChain = new PickUpAvailabilityChain(locationSuccess, board, playerGuidFailPickUp, playerGuidToPieceId);
            Assert.False(pickUpAvailabilityChain.ActionAvailable());
        }
    }
}
