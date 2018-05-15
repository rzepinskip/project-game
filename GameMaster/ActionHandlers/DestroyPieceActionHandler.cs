using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    public class DestroyPieceActionHandler : ActionHandler
    {
        public DestroyPieceActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            return new DestroyAvailabilityChain(PlayerId, Board.Players).ActionAvailable();
        }

        public override BoardData Respond()
        {
            if (!Validate())
                return BoardData.Create(PlayerId, new Piece[0]);

            var player = Board.Players[PlayerId];
            var playerPiece = player.Piece;

            player.Piece = null;
            playerPiece.Type = PieceType.Destroyed;
            Board.Pieces.Remove(playerPiece.Id);

            return BoardData.Create(PlayerId, new[] {playerPiece});
        }
    }
}