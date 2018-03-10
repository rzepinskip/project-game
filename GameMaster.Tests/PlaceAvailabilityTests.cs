using GameMaster.ActionAvailability;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace GameMaster.Tests
{
    public class PlaceAvailabilityTests
    {
        int boardWidth = 5;
        int goalAreaSize = 2;
        int taskAreaSize = 4;
        int pieceId = 1;
        string playerGuidSuccessPlace = "c094cab7-da7b-457f-89e5-a5c51756035f";
        string playerGuidFailPlace = "c094cab7-da7b-457f-89e5-a5c51756035d";
        Dictionary<string, int> playerGuidToPieceId;
        Shared.Board.Location locationFail;
        Shared.Board.Location locationSuccess;
        Shared.Board.Location goalAreaLocation;

        Shared.Board.Board board;

        public PlaceAvailabilityTests() {
            board = new Shared.Board.Board(boardWidth, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.Board.Location() { X = 2, Y = 3 };
            locationSuccess = new Shared.Board.Location() { X = 1, Y = 3 };
            goalAreaLocation = new Shared.Board.Location() { X = 1, Y = 1 };

            board.PlacePieceInTaskArea(1, locationFail);

            playerGuidToPieceId = new Dictionary<string, int>();
            playerGuidToPieceId.Add(playerGuidSuccessPlace, pieceId);
        }

        [Fact]
        public void PlacingPieceOnEmptyField() {
            Assert.True(PlaceAvailability.IsNoPiecePlaced(locationSuccess, board));
        }

        [Fact]
        public void PlacingPieceOnNonEmptyField() {
            Assert.False(PlaceAvailability.IsNoPiecePlaced(locationFail, board));
        }

        [Fact]
        public void PlayerPlaceWhenCarryingPiece() {
            Assert.True(PlaceAvailability.IsPlayerCarryingPiece(playerGuidSuccessPlace, playerGuidToPieceId));
        }

        [Fact]
        public void PlayerPlaceWhenCarryingNoPiece() {
            Assert.False(PlaceAvailability.IsPlayerCarryingPiece(playerGuidFailPlace, playerGuidToPieceId));
        }
    }
}
