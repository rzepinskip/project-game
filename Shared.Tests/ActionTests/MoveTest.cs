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

            AssertPlayerLocation(new Location(2, 3));
        }

        [Fact]
        public void MoveOnPiece()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(2, 2));
            AssertPiece(new Location(2, 2), 2);
        }


        [Fact]
        public void MoveOutsideBoard()
        {
            SetTestedPlayerLocation(new Location(0, 3));

            var message = GetMoveMessage(CommonResources.MoveType.Left);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(0, 3));
        }

        [Fact]
        public void MoveOnEnemyGoalArea()
        {
            SetTestedPlayerLocation(new Location(1, 2));

            var message = GetMoveMessage(CommonResources.MoveType.Down);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(1, 2));
        }

        [Fact]
        public void MoveOnPlayer()
        {
            SetTestedPlayerLocation(new Location(1, 4));

            var message = GetMoveMessage(CommonResources.MoveType.Right);
            if (message.Execute(_gameMasterBoard) is MoveResponse response)
                response.Update(_playerBoard);

            AssertPlayerLocation(new Location(1, 4));
        }

        private void AssertPlayerLocation(Location location)
        {
            AssertPlayerLocationOnBoard(location, _gameMasterBoard);
            AssertPlayerLocationOnBoard(location, _playerBoard);
        }
        private void AssertPlayerLocationOnBoard(Location location, Board board)
        {
            Assert.Equal(PlayerId, board.Content[location.X, location.Y].PlayerId);

            Assert.Equal(board.Players[PlayerId].Location, location);

            foreach (var field in board.Content)
            {
                if ((Location)field != location)
                    Assert.NotEqual(PlayerId, field.PlayerId);
            }
        }

        private void AssertPiece(Location location, int pieceId)
        {
            var piece = _playerBoard.Pieces[pieceId];
            Assert.Equal(pieceId, piece.Id);

            var taskField = _playerBoard.Content[location.X, location.Y] as TaskField;
            Assert.Equal(pieceId, taskField.PieceId);
        }
    }
}
