using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GameMaster.ActionAvailability;
using GameMaster.ActionAvailability.ActionAvailabilityHelpers;

namespace GameMaster.Tests
{
    public class PickUpAvailabilityTests
    {

        int boardWidth = 5;
        int goalAreaSize = 2;
        int taskAreaSize = 4;
        int pieceId = 1;
        string playerGuidSuccessPickUp = "c094cab7-da7b-457f-89e5-a5c51756035f";
        string playerGuidFailPickUp = "c094cab7-da7b-457f-89e5-a5c51756035d";
        Dictionary<string, int> playerGuidToPieceId;
        Shared.Board.Location locationFail;
        Shared.Board.Location locationSuccess;
        Shared.Board.Location goalAreaLocation;

        Shared.Board.Board board;

        public PickUpAvailabilityTests() {
            board = new Shared.Board.Board(boardWidth, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.Board.Location() { X = 2, Y = 3 };
            locationSuccess = new Shared.Board.Location() { X = 1, Y = 3 };
            goalAreaLocation = new Shared.Board.Location() { X = 1, Y = 1 };

            board.PlacePieceInTaskArea(1, locationSuccess);

            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidFailPickUp, pieceId);
        }

        [Fact]
        public void PickUpOnEmptyTaskField()
        {
            Assert.False(PieceRelatedAvailability.IsPieceInCurrentLocation(locationFail, board));
        }
        [Fact]
        public void PickUpOnTaskFieldWithPiece()
        {
            Assert.True(PieceRelatedAvailability.IsPieceInCurrentLocation(locationSuccess, board));
        }
        [Fact]
        public void PickUpOnGoalArea()
        {
            Assert.False(PieceRelatedAvailability.IsPieceInCurrentLocation(goalAreaLocation, board));
        }
        [Fact]
        public void PickUpWhenPlayerNotCarringPiece()
        {
            Assert.True(PieceRelatedAvailability.HasPlayerEmptySlotForPiece(playerGuidSuccessPickUp, playerGuidToPieceId));
        }

        [Fact]
        public void PickUpWhenPlayerCarringPiece() {
            Assert.False(PieceRelatedAvailability.HasPlayerEmptySlotForPiece(playerGuidFailPickUp, playerGuidToPieceId));
        }
    }
}
