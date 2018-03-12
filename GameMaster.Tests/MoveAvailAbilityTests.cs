using Xunit;
using GameMaster.ActionAvailability.ActionAvailabilityHelpers;

namespace GameMaster.Tests
{
    public class MoveAvailabilityTests
    {
        private int boardWidth = 12;
        private int boardHeight = 32;
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;
        private Shared.BoardObjects.Location locationFail;
        private Shared.BoardObjects.Location locationSuccess;

        private Shared.BoardObjects.Board board;

        public MoveAvailabilityTests()
        {
            board = new Shared.BoardObjects.Board(5, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.BoardObjects.Location() { X = 2, Y = 3 };
            locationSuccess = new Shared.BoardObjects.Location() { X = 1, Y = 1 };
        }

        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 20 };
            Assert.False(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 11, Y = 20 };
            Assert.False(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Right, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 0 };
            Assert.False(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 11, Y = 31 };
            Assert.False(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingLeft()
        {
            var l = new Shared.BoardObjects.Location() { X = 1, Y = 20 };
            Assert.True(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingRight()
        {
            var l = new Shared.BoardObjects.Location() { X = 10, Y = 20 };
            Assert.True(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Right, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDown()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 1 };
            Assert.True(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUp()
        {
            var l = new Shared.BoardObjects.Location() { X = 11, Y = 30 };
            Assert.True(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void RedMovingUpToBlueGoal()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 5 };
            Assert.False(MoveAvailability.IsAvailableTeamArea(l, Shared.CommonResources.TeamColour.Red, Shared.CommonResources.MoveType.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void BlueMovingDownToRedGoal()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 2 };
            Assert.False(MoveAvailability.IsAvailableTeamArea(l, Shared.CommonResources.TeamColour.Blue, Shared.CommonResources.MoveType.Down, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void RedMovingUp()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 3 };
            Assert.True(MoveAvailability.IsAvailableTeamArea(l, Shared.CommonResources.TeamColour.Red, Shared.CommonResources.MoveType.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void BlueMovingDown()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 4 };
            Assert.True(MoveAvailability.IsAvailableTeamArea(l, Shared.CommonResources.TeamColour.Blue, Shared.CommonResources.MoveType.Down, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer()
        {
            Assert.False(MoveAvailability.IsFieldPlayerUnoccupied(locationFail, Shared.CommonResources.MoveType.Left, board));
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            Assert.False(MoveAvailability.IsFieldPlayerUnoccupied(locationFail, Shared.CommonResources.MoveType.Right, board));
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            Assert.False(MoveAvailability.IsFieldPlayerUnoccupied(locationFail, Shared.CommonResources.MoveType.Up, board));
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            Assert.False(MoveAvailability.IsFieldPlayerUnoccupied(locationFail, Shared.CommonResources.MoveType.Down, board));
        }

        [Fact]
        public void MovingLeftToUnoccupiedField()
        {
            Assert.True(MoveAvailability.IsFieldPlayerUnoccupied(locationSuccess, Shared.CommonResources.MoveType.Left, board));
        }

        [Fact]
        public void MovingRightToUnoccupiedField()
        {
            Assert.True(MoveAvailability.IsFieldPlayerUnoccupied(locationSuccess, Shared.CommonResources.MoveType.Right, board));
        }

        [Fact]
        public void MovingUpToUnoccupiedField()
        {
            Assert.True(MoveAvailability.IsFieldPlayerUnoccupied(locationSuccess, Shared.CommonResources.MoveType.Up, board));
        }

        [Fact]
        public void MovingDownToUnoccupiedField()
        {
            Assert.True(MoveAvailability.IsFieldPlayerUnoccupied(locationSuccess, Shared.CommonResources.MoveType.Down, board));
        }

        [Fact]
        public void GetNewLocationMovingLeft()
        {
            Assert.Equal(new Shared.BoardObjects.Location() { X = 1, Y = 3 }, MoveAvailability.GetNewLocation(locationFail, Shared.CommonResources.MoveType.Left));
        }

        [Fact]
        public void GetNewLocationMovingRight()
        {
            Assert.Equal(new Shared.BoardObjects.Location() { X = 3, Y = 3 }, MoveAvailability.GetNewLocation(locationFail, Shared.CommonResources.MoveType.Right));
        }

        [Fact]
        public void GetNewLocationMovingUp()
        {
            Assert.Equal(new Shared.BoardObjects.Location() { X = 2, Y = 4 }, MoveAvailability.GetNewLocation(locationFail, Shared.CommonResources.MoveType.Up));
        }

        [Fact]
        public void GetNewLocationMovingDown()
        {
            Assert.Equal(new Shared.BoardObjects.Location() { X = 2, Y = 2 }, MoveAvailability.GetNewLocation(locationFail, Shared.CommonResources.MoveType.Down));
        }
    }
}
