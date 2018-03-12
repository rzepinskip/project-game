using Xunit;
using Shared.BoardObjects;
using Shared.ResponseMessages;
using System.Collections.Generic;

namespace Shared.Tests
{
    public class ResponseMessageTests
    {
        private int boardWidth = 5;
        private int goalAreaSize = 2;
        private int taskAreaSize = 4;

        private Board board;

        public ResponseMessageTests()
        {
            board = new Board(boardWidth, taskAreaSize, goalAreaSize);
            board.Content[1, 2].PlayerId = 1;

            var player1 = new PlayerBase
            {
                Id = 1,
                Team = CommonResources.TeamColour.Red,
                Type = PlayerBase.PlayerType.Member
            };

            var playerInfo1 = new PlayerInfo
            {
                Location = new Location(1,2),
                Piece = null,
                Team = CommonResources.TeamColour.Red
            };

            var piece1 = new Piece
            {
                Id = 1,
                PlayerId = null,
                Type = CommonResources.PieceType.Unknown
            };

            board.PlacePieceInTaskArea(1, new Location(2, 2));

            board.Players.Add(1, playerInfo1);
            board.Pieces.Add(1, piece1);
        }

        [Fact]
        public void CorrectRightMoveResponse()
        {
            //Arrange
            var moveResponse = new MoveResponse
            {
                GameFinished = false,
                NewPlayerLocation = new Location
                {
                    X = 2,
                    Y = 2
                },
                PlayerId = 1,
                TaskFields = new List<TaskField>
                {
                    new TaskField
                    {
                        DistanceToPiece = 0,
                        PieceId = null,
                        PlayerId = null,
                        X = 1,
                        Y = 2
                    }
                }
            };

            //Act
            moveResponse.Update(board);

            //Assert
            Assert.Null(board.Content[1, 2].PlayerId);
            Assert.Equal(1, board.Content[2, 2].PlayerId);
            Assert.Equal(moveResponse.NewPlayerLocation, board.Players[1].Location);
        }

        [Fact]
        public void CorrectPickUpPieceResponse()
        {
            //Arrange
            var piece = board.Pieces[1];
            var pickUpResponse = new PickUpPieceResponse
            {
                Piece = piece,
                GameFinished = false,
                PlayerId = 1
            };

            //Act
            pickUpResponse.Update(board);

            //Assert
            Assert.Equal(1, piece.PlayerId);
            Assert.Equal(piece, board.Players[1].Piece);
        }

        [Fact]
        public void CorrectTestPieceResponse()
        {
            //Arrange
            var piece = board.Pieces[1];
            piece.Type = CommonResources.PieceType.Sham;
            var testPieceResponse = new TestPieceResponse
            {
                Piece = piece,
                GameFinished = false,
                PlayerId = 1
            };

            //Act
            testPieceResponse.Update(board);

            //Assert
            Assert.Equal(piece, testPieceResponse.Piece);
        }

        [Fact]
        public void CorrectDiscoverResponse()
        {
            //Arrange
            var discoverResponce = new DiscoverResponse
            {
                GameFinished = false,
                PlayerId = 1,
            };

            var taskFields = new List<TaskField>();

            for (int i = -1; i <= 1; ++i)
                for (int j = 0; j <= 1; ++j)
                    taskFields.Add(new TaskField
                    {
                        X = 2 + i,
                        Y = 3 + j,
                    });

            discoverResponce.TaskFields = taskFields;

            //Act
            discoverResponce.Update(board);

            //Assert
            for (int i = 0; i < taskFields.Count; ++i)
                Assert.Equal(taskFields[i], board.Content[taskFields[i].X, taskFields[i].Y]);
        }

        [Fact]
        public void CorrectPlacePieceResponse()
        {
            //Arrange
            var goalField = new GoalField
            {
                PlayerId = null,
                X = 2,
                Y = 1,
                Type = CommonResources.GoalFieldType.Goal
            };

            var placePieceResponse = new PlacePieceResponse
            {
                GameFinished = false,
                GoalField = goalField,
                PlayerId = 1
            };

            //Act
            placePieceResponse.Update(board);

            //Arrange
            Assert.Null(board.Players[1].Piece);
            var boardGoalField = board.Content[goalField.X, goalField.Y] as GoalField;
            Assert.Equal(CommonResources.GoalFieldType.NonGoal, boardGoalField.Type);
        }
    }
}
