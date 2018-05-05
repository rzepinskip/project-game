using ClientsCommon.ActionAvailability.Helpers;
using Common;
using Common.BoardObjects;
using Xunit;

namespace ClientsCommon.Tests.ActionAvailability
{
    public class MoveAvailabilityTests
    {
        public MoveAvailabilityTests()
        {
            board = new MockBoard(5, taskAreaSize, goalAreaSize)
            {
                [new Location(1, 3)] = {PlayerId = 1},
                [new Location(3, 3)] = {PlayerId = 2},
                [new Location(2, 4)] = {PlayerId = 3},
                [new Location(2, 2)] = {PlayerId = 4}
            };
            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 1);
        }

        private readonly int boardWidth = 12;
        private readonly int boardHeight = 32;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly Location locationFail;
        private readonly Location locationSuccess;

        private readonly MockBoard board;

        [Fact]
        public void BlueMovingUp()
        {
            var l = new Location(0, 3);
            Assert.True(new MoveAvailability().IsAvailableTeamArea(l, TeamColor.Blue,
                Direction.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void BlueMovingUpToRedGoal()
        {
            var l = new Location(0, 5);
            Assert.False(new MoveAvailability().IsAvailableTeamArea(l, TeamColor.Blue,
                Direction.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void GetNewLocationMovingDown()
        {
            Assert.Equal(new Location(2, 2), locationFail.GetNewLocation(Direction.Down));
        }

        [Fact]
        public void GetNewLocationMovingLeft()
        {
            Assert.Equal(new Location(1, 3), locationFail.GetNewLocation(Direction.Left));
        }

        [Fact]
        public void GetNewLocationMovingRight()
        {
            Assert.Equal(new Location(3, 3), locationFail.GetNewLocation(Direction.Right));
        }

        [Fact]
        public void GetNewLocationMovingUp()
        {
            Assert.Equal(new Location(2, 4), locationFail.GetNewLocation(Direction.Up));
        }

        [Fact]
        public void MovingDown()
        {
            var l = new Location(0, 1);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, Direction.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Location(0, 0);
            Assert.False(
                new MoveAvailability().IsInsideBoard(l, Direction.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, Direction.Down, board));
        }

        [Fact]
        public void MovingDownToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, Direction.Down, board));
        }

        [Fact]
        public void MovingLeft()
        {
            var l = new Location(1, 20);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, Direction.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            var l = new Location(0, 20);
            Assert.False(
                new MoveAvailability().IsInsideBoard(l, Direction.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, Direction.Left, board));
        }

        [Fact]
        public void MovingLeftToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, Direction.Left, board));
        }

        [Fact]
        public void MovingRight()
        {
            var l = new Location(10, 20);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, Direction.Right, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Location(11, 20);
            Assert.False(new MoveAvailability().IsInsideBoard(l, Direction.Right, boardWidth,
                boardHeight));
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, Direction.Right, board));
        }

        [Fact]
        public void MovingRightToUnoccupiedField()
        {
            Assert.True(new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, Direction.Right,
                board));
        }

        [Fact]
        public void MovingUp()
        {
            var l = new Location(11, 30);
            Assert.True(new MoveAvailability().IsInsideBoard(l, Direction.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Location(11, 31);
            Assert.False(new MoveAvailability().IsInsideBoard(l, Direction.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, Direction.Up, board));
        }

        [Fact]
        public void MovingUpToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, Direction.Up, board));
        }

        [Fact]
        public void RedMovingDown()
        {
            var l = new Location(0, 4);
            Assert.True(new MoveAvailability().IsAvailableTeamArea(l, TeamColor.Red,
                Direction.Down, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void RedMovingDownToBlueGoal()
        {
            var l = new Location(0, 2);
            Assert.False(new MoveAvailability().IsAvailableTeamArea(l, TeamColor.Red,
                Direction.Down, goalAreaSize, taskAreaSize));
        }
    }
}