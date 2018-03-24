using Common.BoardObjects;
using Common.Interfaces;
using Xunit;

namespace Common.Tests.ActionTests
{
    public class ActionTest
    {
        protected int PlayerId = 1;
        protected int OtherPlayerId = 2;

        protected IBoard GameMasterBoard;
        protected IBoard PlayerBoard;

        public ActionTest()
        {
            GameMasterBoard = GenerateGameMasterBoard();
            PlayerBoard = GenerateBoard();
        }

        private IBoard GenerateGameMasterBoard()
        {
            var board = GenerateBoard();

            var piece1 = new Piece(1, PieceType.Sham);
            var piece2 = new Piece(2, PieceType.Normal);

            board.Pieces.Add(piece1.Id,piece1);
            ((TaskField) board[new Location(0, 5)]).PieceId = piece1.Id;

            board.Pieces.Add(piece2.Id, piece2);
            ((TaskField)board[new Location(2,2)]).PieceId = piece2.Id;

            var playerInfo = new PlayerInfo(TeamColor.Red, PlayerType.Member, new Location(0, 4));
            board.Players.Add(OtherPlayerId, playerInfo);
            board[playerInfo.Location].PlayerId = OtherPlayerId;


            return board;
        }
        private IBoard GenerateBoard()
        {
            var board = new MockBoard(4, 4, 2);

            return board;
        }

        protected void SetTestedPlayerLocation(Location location)
        {
            var testedPlayerInfo = new PlayerInfo(TeamColor.Red, PlayerType.Member, location);
            GameMasterBoard.Players.Add(PlayerId, testedPlayerInfo);
            PlayerBoard.Players.Add(PlayerId, testedPlayerInfo);

            GameMasterBoard[location].PlayerId = PlayerId;
            PlayerBoard[location].PlayerId = PlayerId;

            GameMasterBoard.Players[PlayerId].Location = location;
            PlayerBoard.Players[PlayerId].Location = location;
        }


        protected void AssertPlayerLocation(Location location, int playerId)
        {
            AssertPlayerLocationOnBoard(location, GameMasterBoard, playerId);
            AssertPlayerLocationOnBoard(location, PlayerBoard, playerId);
        }
        protected void AssertPlayerLocationOnBoard(Location location, IBoard board, int playerId)
        {
            Assert.Equal(playerId, board[location].PlayerId);

            for (int i = 0; i < board.Width; i++)
                for (int j = 0; j < board.Height; j++)
                {
                    var field = board[new Location(i, j)];

                    if (field != location)
                        Assert.NotEqual(playerId, field.PlayerId);
                }
        }

        protected void AssertPiece(Location location, int pieceId)
        {
            var piece = PlayerBoard.Pieces[pieceId];
            Assert.Equal(pieceId, piece.Id);

            var taskField = PlayerBoard[location] as TaskField;
            Assert.Equal(pieceId, taskField.PieceId);
        }
    }
}
