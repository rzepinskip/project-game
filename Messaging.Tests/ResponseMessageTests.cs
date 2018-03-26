using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Xunit;
using Messaging.Responses;

namespace Messaging.Tests
{
    public class ResponseTests
    {
        private readonly int _playerId = 1;
        private readonly int _pieceId = 2;

        public ResponseTests()
        {
            _board = new MockBoard(_boardWidth, _taskAreaSize, _goalAreaSize);

            var player1 = new PlayerBase
            {
                Id = _playerId,
                Team = TeamColor.Red,
                Type = PlayerType.Member
            };
            var playerInfo1 = new PlayerInfo(TeamColor.Red, PlayerType.Member, new Location(1, 2));

            _board[playerInfo1.Location].PlayerId = player1.Id;
            _board.Players.Add(player1.Id, playerInfo1);


            var piece = new Piece(_pieceId, PieceType.Unknown);
            _board.Pieces.Add(piece.Id, piece);

            var taskField = (TaskField)_board[new Location(2, 2)];
            taskField.PieceId = piece.Id;
        }

        private readonly int _boardWidth = 5;
        private readonly int _goalAreaSize = 2;
        private readonly int _taskAreaSize = 4;

        private readonly IBoard _board;

        [Fact]
        public void CorrectDiscoverResponse()
        {
            //Arrange
            var taskFields = new List<TaskField>();

            for (var i = -1; i <= 1; ++i)
                for (var j = 0; j <= 1; ++j)
                    taskFields.Add(new TaskField(new Location(2 + i, 3 + j)));

            var discoverResponce = new DiscoverResponse(_playerId, taskFields, new List<Piece>());

            //Act
            discoverResponce.Update(_board);

            //Assert
            foreach (var taskField in taskFields)
                Assert.Equal(taskField, _board[taskField]);
        }

        [Fact]
        public void CorrectPickUpPieceResponse()
        {
            //Arrange
            var piece = _board.Pieces[_pieceId];
            var pickUpResponse = new PickUpPieceResponse(_playerId, piece);

            //Act
            pickUpResponse.Update(_board);

            //Assert
            Assert.Equal(_pieceId, piece.Id);
            Assert.Equal(piece, _board.Players[_playerId].Piece);
        }

        [Fact]
        public void CorrectPlacePieceResponse()
        {
            //Arrange
            var goalField = new GoalField(new Location(2, 1), TeamColor.Red, GoalFieldType.Goal);

            var placePieceResponse = new PlacePieceResponse(_playerId, goalField);

            //Act
            placePieceResponse.Update(_board);

            //Arrange
            Assert.Null(_board.Players[_playerId].Piece);
            var boardGoalField = (GoalField)_board[goalField];
            Assert.Equal(GoalFieldType.NonGoal, boardGoalField.Type);
        }

        [Fact]
        public void CorrectRightMoveResponse()
        {
            //Arrange
            var oldPlayerLocation = _board.Players[_playerId].Location;
            var newLocation = new Location(2, 2);
            var taskFields = new List<TaskField>
            {
                new TaskField(new Location(2,2))
            };

            var moveResponse = new MoveResponse(_playerId, newLocation, taskFields, new List<Piece>());

            //Act
            moveResponse.Update(_board);

            //Assert
            Assert.Null(_board[oldPlayerLocation].PlayerId);
            Assert.Equal(_playerId, _board[newLocation].PlayerId);
            Assert.Equal(moveResponse.NewPlayerLocation, _board.Players[_playerId].Location);
        }

        [Fact]
        public void CorrectTestPieceResponse()
        {
            //Arrange
            var piece = _board.Pieces[_pieceId];
            piece.Type = PieceType.Sham;
            var testPieceResponse = new TestPieceResponse(_playerId, piece);

            //Act
            testPieceResponse.Update(_board);

            //Assert
            Assert.Equal(piece, testPieceResponse.Piece);
        }
    }
}