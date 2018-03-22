using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    internal class TestPieceResponse : Response
    {
        public TestPieceResponse(int playerId, Piece piece, bool isGameFinished = false) : base(playerId,
            isGameFinished)
        {
            Piece = piece;
        }

        public Piece Piece { get; }

        public override void Update(IBoard board)
        {
            var playerInfo = board.Players[PlayerId];
            playerInfo.Piece = Piece;
            if (Piece.Type == PieceType.Sham)
                playerInfo.Piece = null;
        }
    }
}