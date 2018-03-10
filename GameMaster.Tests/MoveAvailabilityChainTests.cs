using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GameMaster.ActionAvailability;
using static Shared.CommonResources;

namespace GameMaster.Tests
{
    public class MoveAvailabilityChainTests
    {
        int goalAreaSize = 2;
        int taskAreaSize = 4;
        Shared.Board.Location locationFail;
        Shared.Board.Location locationSuccess;
        Shared.Board.Location locationMovingFail;
        Shared.Board.Board board;
        Team team = Team.Red; 

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
            MoveAvailabilityChain moveAvailabilityChain = new MoveAvailabilityChain(locationMovingFail, Shared.CommonResources.MoveType.Left, team, board);
            Assert.False(moveAvailabilityChain.MoveAvailable());

        }
        
        [Fact]
        public void RedMovingUpToBlueGoal() 
        {
            Shared.Board.Location l = new Shared.Board.Location() { X = 0, Y = 5 };
            MoveAvailabilityChain moveAvailabilityChain = new MoveAvailabilityChain(l, MoveType.Up, Team.Red, board);
            Assert.False(moveAvailabilityChain.MoveAvailable());
        }

        [Fact]
        public void MovingLeftToFieldWithPlayer() 
        {
            MoveAvailabilityChain moveAvailabilityChain = new MoveAvailabilityChain(locationFail, MoveType.Left, Team.Red, board);
            Assert.False(moveAvailabilityChain.MoveAvailable());
        }

        [Fact]
        public void MoveAvailable() 
        {
            MoveAvailabilityChain moveAvailabilityChain = new MoveAvailabilityChain(locationSuccess, MoveType.Left, Team.Red, board);
            Assert.True(moveAvailabilityChain.MoveAvailable());
        }
    }

}
