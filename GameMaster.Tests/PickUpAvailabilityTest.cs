using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GameMaster.ActionAvailability;

namespace GameMaster.Tests
{
    public class PickUpAvailabilityTest
    {

        int boardWidth = 5;
        int goalAreaSize = 2;
        int taskAreaSize = 4;
        string playerGuid = "c094cab7-da7b-457f-89e5-a5c51756035f";
        Dictionary<string, int> playerGuidToPieceId;
        Shared.Board.Location locationFail;
        Shared.Board.Location locationSuccess;
        Shared.Board.Location goalAreaLocation;

        Shared.Board.Board board;

        public PickUpAvailabilityTest() {
            board = new Shared.Board.Board(boardWidth, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.Board.Location() { X = 2, Y = 3 };
            locationSuccess = new Shared.Board.Location() { X = 1, Y = 3 };
            goalAreaLocation = new Shared.Board.Location() { X = 1, Y = 1 };

            board.PlacePieceInTaskArea(1, locationSuccess);
        }

        [Fact]
        public void PickUpOnEmptyTaskField()
        {
            Assert.False(PickUpAvailability.IsPieceInCurrentLocation(locationFail, board));
        }
        [Fact]
        public void PickUpOnTaskFieldWithPiece()
        {
            Assert.True(PickUpAvailability.IsPieceInCurrentLocation(locationSuccess, board));
        }
        [Fact]
        public void PickUpOnGoalArea()
        {
            Assert.False(PickUpAvailability.IsPieceInCurrentLocation(goalAreaLocation, board));
        }
        [Fact]
        public void PickUpWhenPlayerNotCarringPiece()
        {
            Assert.True(PickUpAvailability.HasPlayerEmptySlotForPiece())
        }
    }
}
