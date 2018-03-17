using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Tests.ActionTests
{
    public class ActionTest
    {
        protected int PlayerId = 1;

        protected Board _gameMasterBoard;
        protected Board _playerBoard;

        public ActionTest()
        {
            _gameMasterBoard = GenerateGameMasterBoard();
            _playerBoard = BoardGeneratePlayerBoard();
        }

        private Board GenerateGameMasterBoard()
        {
            var board = GenerateBoard();

            var piece1 = new Piece
            {
                Id = 1,
                Type = CommonResources.PieceType.Sham
            };

            var piece2 = new Piece
            {
                Id = 2,
                Type = CommonResources.PieceType.Normal
            };

            board.PlacePieceInTaskArea(piece1.Id, new Location(0, 6));
            board.PlacePieceInTaskArea(piece2.Id, new Location(2,2));
            board.Pieces.Add(piece1.Id, piece1);
            board.Pieces.Add(piece2.Id, piece2);

            var testedPlayerInfo = new PlayerInfo
            {
                Team = CommonResources.TeamColour.Red
            };
            board.Players.Add(PlayerId, testedPlayerInfo);

            var playerId = 2;
            var playerInfo = new PlayerInfo
            {
                Team = CommonResources.TeamColour.Blue,
                Location = new Location(2,4),
                
            };
            board.Players.Add(playerId, playerInfo);
            board.Content[playerInfo.Location.X, playerInfo.Location.Y].PlayerId = playerId;


            return board;
        }
        private Board BoardGeneratePlayerBoard()
        {
            var board = GenerateBoard();

            var playerInfo = new PlayerInfo
            {
                Team = CommonResources.TeamColour.Red
            };

            board.Players.Add(PlayerId, playerInfo);


            return board;
        }
        private Board GenerateBoard()
        {
            var board = new Board(4, 4, 2);

            return board;
        }

        protected void SetTestedPlayerLocation(Location location)
        {
            _gameMasterBoard.Content[location.X, location.Y].PlayerId = PlayerId;
            _playerBoard.Content[location.X, location.Y].PlayerId = PlayerId;

            _gameMasterBoard.Players[PlayerId].Location = location;
            _playerBoard.Players[PlayerId].Location = location;
        }
    }
}
