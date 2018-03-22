using Common.BoardObjects;
using Xunit;

namespace Common.Tests
{
    public class BoardTests
    {
        public BoardTests()
        {
            board = new MockBoard(boardWidth, taskAreaSize, goalAreaSize);
            locationInTaskArea = new Location(3, 3);
            locationInRedGoal = new Location(3, 0);
            locationInBlueGoal = new Location(3, 7);
            locationOutsideBoard = new Location(10, 10);
            pieceLocation = new Location(2, 3);
            nonPieceLocation = new Location(1, 3);
            placePieceSuccess = new Location(4, 3);

            board.PlacePieceInTaskArea(pieceId, pieceLocation);
        }

        private readonly MockBoard board;
        private readonly Location locationInTaskArea;
        private readonly Location locationInRedGoal;
        private readonly Location locationInBlueGoal;
        private readonly Location locationOutsideBoard;
        private readonly Location pieceLocation;
        private readonly Location nonPieceLocation;
        private readonly Location placePieceSuccess;
        private readonly int boardWidth = 5;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly int pieceId = 1;
        private readonly int anotherPieceId = 2;

        [Fact]
        public void GetExistingPieceFromBoard()
        {
            int? anotherPieceValue = pieceId;
            Assert.Equal(anotherPieceValue, board.GetPieceIdAt(pieceLocation));
        }

        [Fact]
        public void GetNonExistingPieceFromBoard()
        {
            Assert.Null(board.GetPieceIdAt(nonPieceLocation));
        }

        [Fact]
        public void LocationInTaskAreaWhenInBlueGoalArea()
        {
            Assert.False(board.IsLocationInTaskArea(locationInBlueGoal));
        }

        [Fact]
        public void LocationInTaskAreaWhenInRedGoalArea()
        {
            Assert.False(board.IsLocationInTaskArea(locationInRedGoal));
        }

        [Fact]
        public void LocationInTaskAreaWhenInTaskArea()
        {
            Assert.True(board.IsLocationInTaskArea(locationInTaskArea));
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
            Assert.Equal(anotherPieceIdValue,
                (board[placePieceSuccess] as TaskField).PieceId);
        }
    }
}