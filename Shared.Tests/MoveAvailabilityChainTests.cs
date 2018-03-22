using Shared.ActionAvailability.AvailabilityChain;
using Shared.BoardObjects;
using Xunit;
using static Shared.CommonResources;

namespace Shared.Tests
{
    public class MoveAvailabilityChainTests
    {
        public MoveAvailabilityChainTests()
        {
            board = new Board(5, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Location(2, 3);
            locationSuccess = new Location(1, 1);
        }

        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly Location locationFail;
        private readonly Location locationSuccess;
        private readonly Board board;
        private readonly TeamColour team = TeamColour.Blue;
        private readonly TeamColour opposingTeam = TeamColour.Red;

        [Fact]
        public void BlueMovingUpToRedGoal()
        {
            var l = new Location(0, 5);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableDown()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Down, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }


        [Fact]
        public void MoveAvailableLeft()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Left, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableRight()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Right, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableUp()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Up, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Location(0, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            var l = new Location(0, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Location(4, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Location(0, 7);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, opposingTeam, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Up, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void RedMovingDownToBlueGoal()
        {
            var l = new Location(0, 2);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Down, opposingTeam, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }
    }
}