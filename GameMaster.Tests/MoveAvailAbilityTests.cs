using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using GameMaster.ActionAvailability;

namespace GameMaster.Tests
{
    public class MoveAvailAbilityTests
    {
        [Fact]
        public void MovingLeftAndLeavingBoard()
        {
            Shared.Board.Location l = new Shared.Board.Location() { X = 0, Y = 20 };
            Assert.False(MoveAvailability.IsInsideBoard(l, Shared.CommonResources.MoveType.Left, 12, 32));
        }
    }
}
