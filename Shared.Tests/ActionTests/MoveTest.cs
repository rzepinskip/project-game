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
            SetTestedPlayerLocation(new Location(1,3));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            Assert.True(AssertPlayerLocation(new Location(2,3)));
        }

        [Fact]
        public void MoveOnPiece()
        {
            SetTestedPlayerLocation(new Location(1,2));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            Assert.True(AssertPlayerLocation(new Location(2,2)));
            Assert.True(AssertPiece(new Location(2,2), 2));
        }


        [Fact]
        public void MoveOutsideBoard()
        {
            SetTestedPlayerLocation(new Location(0,3));

            var message = GetMoveMessage(CommonResources.MoveType.Left);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            Assert.True(AssertPlayerLocation(new Location(0,3)));
        }

        [Fact]
        public void MoveOnEnemyGoalArea()
        {
            SetTestedPlayerLocation(new Location(1,2));

            var message = GetMoveMessage(CommonResources.MoveType.Down);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            Assert.True(AssertPlayerLocation(new Location(1,2)));
        }

        [Fact]
        public void MoveOnPlayer()
        {
            SetTestedPlayerLocation(new Location(1,4));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            Assert.True(AssertPlayerLocation(new Location(1,4)));
        }

        private bool AssertPlayerLocation(Location location)
        {
            if (location == null)
                return false;

            return AssertPlayerLocationOnBoard(location, _gameMasterBoard) && AssertPlayerLocationOnBoard(location, _playerBoard);

        }
        private bool AssertPlayerLocationOnBoard(Location location, Board board)
        {
            if (board.Content[location.X, location.Y].PlayerId != PlayerId)
                return false;

            if (board.Players[PlayerId].Location != location)
                return false;

            foreach (var field in board.Content)
            {
                if ((Location)field != location && field.PlayerId == PlayerId)
                    return false;
            }

            return true;
        }

        private bool AssertPiece(Location location, int pieceId)
        {
            if (location == null)
                return false;

            var piece = _playerBoard.Pieces[pieceId];
            if (piece == null || piece.Id != pieceId)
                return false;

            if (_playerBoard.Content[location.X, location.Y] is TaskField taskField)
                if (taskField.PieceId == pieceId)
                    return true;

            return false;
        }
    }
}
