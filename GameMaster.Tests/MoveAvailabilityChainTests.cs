using Xunit;
using static Shared.CommonResources;
using GameMaster.ActionAvailability.AvailabilityChain;

namespace GameMaster.Tests
{
    public class MoveAvailabilityChainTests
    {
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;
        private Shared.BoardObjects.Location locationFail;
        private Shared.BoardObjects.Location locationSuccess;
        private Shared.BoardObjects.Board board;
        private TeamColour team = TeamColour.Red; 

        public MoveAvailabilityChainTests() 
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
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 0 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 4, Y = 0 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 0 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpAndLeavingBoard()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 7 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, TeamColour.Blue, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void RedMovingUpToBlueGoal() 
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 5 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, TeamColour.Red, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void BlueMovingDownToRedGoal()
        {
            var l = new Shared.BoardObjects.Location() { X = 0, Y = 2 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Down, TeamColour.Blue, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer() 
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingRightToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Right, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingUpToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Up, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingDownToFieldWithPlayer()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Down, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
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
        public void MoveAvailableDown()
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Down, team, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }
    }

}
