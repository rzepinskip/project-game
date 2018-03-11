using Xunit;
using static Shared.CommonResources;
using GameMaster.ActionAvailability.AvailabilityChain;

namespace GameMaster.Tests
{
    public class MoveAvailabilityChainTests
    {
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;
        private Shared.Board.Location locationFail;
        private Shared.Board.Location locationSuccess;
        private Shared.Board.Location locationMovingFail;
        private Shared.Board.Board board;
        private Team team = Team.Red; 

        public MoveAvailabilityChainTests() 
        {
            board = new Shared.Board.Board(5, taskAreaSize, goalAreaSize);
            board.Content[1, 3].PlayerId = 1;
            board.Content[3, 3].PlayerId = 2;
            board.Content[2, 4].PlayerId = 3;
            board.Content[2, 2].PlayerId = 4;
            locationFail = new Shared.Board.Location() { X = 2, Y = 3 };
            locationSuccess = new Shared.Board.Location() { X = 1, Y = 1 };
            locationMovingFail = new Shared.Board.Location() { X = 0, Y = 0 };
        }

        [Fact]
        public void MovingLeftAndLeavingBoard() 
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationMovingFail, MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());

        }
        
        [Fact]
        public void RedMovingUpToBlueGoal() 
        {
            var l = new Shared.Board.Location() { X = 0, Y = 5 };
            var moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, Team.Red, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer() 
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Left, Team.Red, board);
            Assert.False(moveAvailabilityChain.ActionAvailable());
        }

        [Fact]
        public void MoveAvailable() 
        {
            var moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Left, Team.Red, board);
            Assert.True(moveAvailabilityChain.ActionAvailable());
        }
    }

}
