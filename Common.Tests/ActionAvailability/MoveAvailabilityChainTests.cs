using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;
using Xunit;

namespace ClientsCommon.Tests.ActionAvailability
{
    public class MoveAvailabilityChainTests
    {
        public MoveAvailabilityChainTests()
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

        private readonly int goalAreaSize = 2;
        private readonly int taskAreaSize = 4;
        private readonly Location locationFail;
        private readonly Location locationSuccess;
        private readonly MockBoard board;
        private readonly TeamColor team = TeamColor.Blue;
        private readonly TeamColor opposingTeam = TeamColor.Red;

        [Fact]
        public void BlueMovingUpToRedGoal()
        {
            var l = new Location(0, 5);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Up, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableDown()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, Direction.Down, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }


        [Fact]
        public void MoveAvailableLeft()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, Direction.Left, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableRight()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, Direction.Right, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailableUp()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, Direction.Up, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Location(0, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, Direction.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            var l = new Location(0, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, Direction.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Location(4, 0);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, Direction.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Location(0, 7);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Up, opposingTeam, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, Direction.Up, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void RedMovingDownToBlueGoal()
        {
            var l = new Location(0, 2);
            var moveAvailabilityChain = new MoveAvailabilityChain(l, Direction.Down, opposingTeam, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }
    }
}