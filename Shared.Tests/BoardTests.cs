using Shared.BoardObjects;
using Xunit;

namespace Shared.Tests
{
    public class BoardTests
    {
        private BoardObjects.Board board;
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
            board = new BoardObjects.Board(boardWidth, taskAreaSize, goalAreaSize);
            locationInTaskArea = new Location(3, 3);
            locationInRedGoal = new Location(3, 0);
            locationInBlueGoal = new Location(3, 7);
            locationOutsideBoard = new Location(10, 10);
            pieceLocation = new Location(2, 3);
            nonPieceLocation = new Location(1, 3);
            placePieceSuccess = new Location(4, 3);

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
