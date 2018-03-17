using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Shared.Tests.ActionTests
{
    public class MoveTest : ActionTest
    {
        private Move GetMoveMessage(CommonResources.MoveType direction)
        {
            return new Move()
            {
                PlayerId = this.PlayerId,
                Direction = direction
            };

        }

        [Fact]
        public void BasicMove()
        {
            SetTestedPlayerLocation(new Location(1, 3));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(2, 3), PlayerId);
        }

        [Fact]
        public void MoveOnPiece()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(2, 2), PlayerId);
            AssertPiece(new Location(2, 2), 2);
        }


        [Fact]
        public void MoveOutsideBoard()
        {
            SetTestedPlayerLocation(new Location(0, 3));

            var message = GetMoveMessage(CommonResources.MoveType.Left);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(0, 3), PlayerId);
        }

        [Fact]
        public void MoveOnEnemyGoalArea()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = GetMoveMessage(CommonResources.MoveType.Down);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(1, 2), PlayerId);
        }

        [Fact]
        public void MoveOnPlayer()
        {
            SetTestedPlayerLocation(new Location(1, 4));

            var message = GetMoveMessage(CommonResources.MoveType.Left);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(1, 4), PlayerId);
        }
    }
}
