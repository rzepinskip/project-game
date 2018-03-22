using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Resources;
using Xunit;
using Messaging.Responses;

namespace Messaging.Tests
{
    public class ResponseMessageTests
    {
        private readonly int _playerId = 1;
        private readonly int _pieceId = 2;

        public ResponseMessageTests()
        {
            board = new MockBoard(_boardWidth, _taskAreaSize, _goalAreaSize);

            var player1 = new PlayerBase
            {
                Id = _playerId,
                Team = TeamColor.Red,
                Type = PlayerType.Member
            };
            var playerInfo1 = new PlayerInfo(TeamColor.Red, PlayerType.Member, new Location(1, 2));

            board[playerInfo1.Location].PlayerId = player1.Id;
            board.Players.Add(player1.Id, playerInfo1);

            var piece1 = new Piece(_pieceId, PieceType.Unknown);
            board.Pieces.Add(piece1.Id, piece1);
            ((TaskField)(board[new Location(2, 2)])).PieceId = piece1.Id;

        }

        private readonly int _boardWidth = 5;
        private readonly int _goalAreaSize = 2;
        private readonly int _taskAreaSize = 4;

        private readonly IBoard board;

        [Fact]
        public void CorrectDiscoverResponse()
        {
            //Arrange

            var taskFields = new List<TaskField>();

            for (var i = -1; i <= 1; ++i)
                for (var j = 0; j <= 1; ++j)
                    taskFields.Add(new TaskField(new Location(2 + i, 3 + j), -1));

            var discoverResponce = new DiscoverResponse(_playerId, taskFields, null);

            //Act
            discoverResponce.Update(board);

            //Assert
            for (var i = 0; i < taskFields.Count; ++i)
                Assert.Equal(taskFields[i], board[taskFields[i]]);
        }

        [Fact]
        public void CorrectPickUpPieceResponse()
        {
            //Arrange
            var piece = board.Pieces[_pieceId];
            var pickUpResponse = new PickUpPieceResponse(_playerId, piece);

            //Act
            pickUpResponse.Update(board);

            //Assert
            Assert.Equal(_pieceId, piece.PlayerId);
            Assert.Equal(piece, board.Players[_playerId].Piece);
        }

        [Fact]
        public void CorrectPlacePieceResponse()
        {
            //Arrange
            var goalField = new GoalField(new Location(2, 1), TeamColor.Red, GoalFieldType.Goal);

            var placePieceResponse = new PlacePieceResponse(_playerId, goalField);

            //Act
            placePieceResponse.Update(board);

            //Arrange
            Assert.Null(board.Players[1].Piece);
            var boardGoalField = board[goalField] as GoalField;
            Assert.Equal(GoalFieldType.NonGoal, boardGoalField.Type);
        }

        [Fact]
        public void CorrectRightMoveResponse()
        {
            //Arrange
            var newLocation = new Location(2, 2);
            var taskFields = new List<TaskField>
            {
                new TaskField(new Location(1,2),-1)
            };

            var moveResponse = new MoveResponse(_playerId, newLocation, taskFields, null);

            //Act
            moveResponse.Update(board);

            var playerLocation = board.Players[0].Location;
            //Assert
            Assert.Null(board[playerLocation].PlayerId);
            Assert.Equal(1, board[playerLocation].PlayerId);
            Assert.Equal(moveResponse.NewPlayerLocation, board.Players[1].Location);
        }

        [Fact]
        public void CorrectTestPieceResponse()
        {
            //Arrange
            var piece = board.Pieces[_pieceId];
            piece.Type = PieceType.Sham;
            var testPieceResponse = new TestPieceResponse(_playerId, piece);

            //Act
            testPieceResponse.Update(board);

            //Assert
            Assert.Equal(piece, testPieceResponse.Piece);
        }
    }
}