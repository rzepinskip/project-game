using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    public class TestPieceResponse : Response
    {
        public TestPieceResponse(int playerId, Piece piece, bool isGameFinished = false) : base(playerId,
            isGameFinished)
        {
            Piece = piece;
        }

        public Piece Piece { get; }

        public override void Update(IBoard board)
        {
            if (Piece == null)
                return;

            var playerInfo = board.Players[PlayerId];
            playerInfo.Piece = Piece;
            if (Piece.Type == PieceType.Sham)
                playerInfo.Piece = null;
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Test, base.ToLog());
        }
    }
}