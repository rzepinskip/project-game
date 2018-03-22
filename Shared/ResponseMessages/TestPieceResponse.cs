using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class TestPieceResponse : ResponseMessage
    {
        public Piece Piece { get; set; }

        public override void Update(Board board)
        {
            var playerInfo = board.Players[PlayerId];
            playerInfo.Piece = Piece;
            if (Piece.Type == CommonResources.PieceType.Sham)
                playerInfo.Piece = null;
        }
    }
}