using Common.BoardObjects;
using Common.Interfaces;

namespace Messaging.Responses
{
    public class PickUpPieceResponse : Response
    {
        public PickUpPieceResponse(int playerId, Piece piece = null, bool isGameFinished = false) : base(playerId,
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
            Piece.PlayerId = PlayerId;
        }
    }
}