using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using Xunit;

namespace Shared.Tests
{
    public class MoveAvailabilityTests
    {
        public MoveAvailabilityTests()
        {
            board = new Board(5, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 1);
        }

        private readonly int boardWidth = 12;
        private readonly int boardHeight = 32;
        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly Location locationFail;
        private readonly Location locationSuccess;

        private readonly Board board;

        [Fact]
        public void BlueMovingUp()
        {
            var l = new Location(0, 3);
            Assert.True(new MoveAvailability().IsAvailableTeamArea(l, CommonResources.TeamColour.Blue,
                CommonResources.MoveType.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void BlueMovingUpToRedGoal()
        {
            var l = new Location(0, 5);
            Assert.False(new MoveAvailability().IsAvailableTeamArea(l, CommonResources.TeamColour.Blue,
                CommonResources.MoveType.Up, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void GetNewLocationMovingDown()
        {
            Assert.Equal(new Location(2, 2), locationFail.GetNewLocation(CommonResources.MoveType.Down));
        }

        [Fact]
        public void GetNewLocationMovingLeft()
        {
            Assert.Equal(new Location(1, 3), locationFail.GetNewLocation(CommonResources.MoveType.Left));
        }

        [Fact]
        public void GetNewLocationMovingRight()
        {
            Assert.Equal(new Location(3, 3), locationFail.GetNewLocation(CommonResources.MoveType.Right));
        }

        [Fact]
        public void GetNewLocationMovingUp()
        {
            Assert.Equal(new Location(2, 4), locationFail.GetNewLocation(CommonResources.MoveType.Up));
        }

        [Fact]
        public void MovingDown()
        {
            var l = new Location(0, 1);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Location(0, 0);
            Assert.False(
                new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Down, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, CommonResources.MoveType.Down, board));
        }

        [Fact]
        public void MovingDownToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, CommonResources.MoveType.Down, board));
        }

        [Fact]
        public void MovingLeft()
        {
            var l = new Location(1, 20);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            var l = new Location(0, 20);
            Assert.False(
                new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Left, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, CommonResources.MoveType.Left, board));
        }

        [Fact]
        public void MovingLeftToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, CommonResources.MoveType.Left, board));
        }

        [Fact]
        public void MovingRight()
        {
            var l = new Location(10, 20);
            Assert.True(
                new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Right, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Location(11, 20);
            Assert.False(new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Right, boardWidth,
                boardHeight));
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, CommonResources.MoveType.Right, board));
        }

        [Fact]
        public void MovingRightToUnoccupiedField()
        {
            Assert.True(new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, CommonResources.MoveType.Right,
                board));
        }

        [Fact]
        public void MovingUp()
        {
            var l = new Location(11, 30);
            Assert.True(new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Location(11, 31);
            Assert.False(new MoveAvailability().IsInsideBoard(l, CommonResources.MoveType.Up, boardWidth, boardHeight));
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            Assert.False(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationFail, CommonResources.MoveType.Up, board));
        }

        [Fact]
        public void MovingUpToUnoccupiedField()
        {
            Assert.True(
                new MoveAvailability().IsFieldPlayerUnoccupied(locationSuccess, CommonResources.MoveType.Up, board));
        }

        [Fact]
        public void RedMovingDown()
        {
            var l = new Location(0, 4);
            Assert.True(new MoveAvailability().IsAvailableTeamArea(l, CommonResources.TeamColour.Red,
                CommonResources.MoveType.Down, goalAreaSize, taskAreaSize));
        }

        [Fact]
        public void RedMovingDownToBlueGoal()
        {
            var l = new Location(0, 2);
            Assert.False(new MoveAvailability().IsAvailableTeamArea(l, CommonResources.TeamColour.Red,
                CommonResources.MoveType.Down, goalAreaSize, taskAreaSize));
        }
    }
}