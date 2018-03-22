using System.Xml.Serialization;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPieceRequest : Request
    {
        protected PickUpPieceRequest(int playerId) : base(playerId)
        {
        }

        public override Response Execute(IBoard board)
        {
            var response = new PickUpPieceResponse(PlayerId);

            var player = board.Players[PlayerId];
            if (!board.IsLocationInTaskArea(player.Location))
                return response;

            var playerField = board[player.Location] as TaskField;

            if (!playerField.PieceId.HasValue)
                return response;

            var piece = board.Pieces[playerField.PieceId.Value];
            piece.PlayerId = PlayerId;

            player.Piece = piece;
            playerField.PieceId = null;

            response = new PickUpPieceResponse(PlayerId, new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));

            return response;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.PickUp);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PickUpDelay;
        }
    }
}