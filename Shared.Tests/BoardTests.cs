using Shared.Board;
using Xunit;

namespace Shared.Tests
{
    public class BoardTests
    {
        private Board.Board board;
        private Location locationInTaskArea;
        private Location locationInRedGoal;
        private Location locationInBlueGoal;
        private Location locationOutsideBoard;
        private Location pieceLocation;
        private Location nonPieceLocation;
        private Location placePieceSuccess;
        private int boardWidth = 5;
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;
        private int pieceId = 1;
        private int anotherPieceId = 2;

        public BoardTests()
        {
            board = new Board.Board(boardWidth, taskAreaSize, goalAreaSize);
            locationInTaskArea = new Location() { X = 3, Y = 3 };
            locationInRedGoal = new Location() { X = 3, Y = 0 };
            locationInBlueGoal = new Location() { X = 3, Y = 7 };
            locationOutsideBoard = new Location() { X = 10, Y = 10 };
            pieceLocation = new Location() { X = 2, Y = 3 };
            nonPieceLocation = new Location() { X = 1, Y = 3 };
            placePieceSuccess = new Location() { X = 4, Y = 3 };

            board.PlacePieceInTaskArea(pieceId, pieceLocation);
        }

        [Fact]
        public void LocationInTaskAreaWhenInTaskArea()
        {
            Assert.True(board.IsLocationInTaskArea(locationInTaskArea));
        }

        [Fact]
        public void LocationInTaskAreaWhenInRedGoalArea()
        {
            Assert.False(board.IsLocationInTaskArea(locationInRedGoal));
        }

        [Fact]
        public void LocationInTaskAreaWhenInBlueGoalArea()
        {
            Assert.False(board.IsLocationInTaskArea(locationInBlueGoal));
        }

        [Fact]
        public void LocationInTaskAreaWhenOutsideBoard()
        {
            Assert.False(board.IsLocationInTaskArea(locationOutsideBoard));
        }

        [Fact]
        public void PlacePieceWhenInTaskArea()
        {
            board.PlacePieceInTaskArea(anotherPieceId, placePieceSuccess);
            int? anotherPieceIdValue = anotherPieceId;
            Assert.Equal(anotherPieceIdValue, ((TaskField)board.Content[placePieceSuccess.X, placePieceSuccess.Y]).PieceId);
        }

        [Fact]
        public void GetExistingPieceFromBoard()
        {
            int? anotherPieceValue = pieceId;
            Assert.Equal(anotherPieceValue, board.GetPieceFromBoard(pieceLocation));
        }

        [Fact]
        public void GetNonExistingPieceFromBoard()
        {
            Assert.Null(board.GetPieceFromBoard(nonPieceLocation));
        }

    }
}
