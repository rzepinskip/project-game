using System.Xml.Serialization;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Common.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPieceRequest : Request
    {
        public override Response Execute(IBoard board)
        {
            var response = new PickUpPieceResponse();

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

            response = new PickUpPieceResponse
            {
                PlayerId = PlayerId,
                Piece = new Piece(piece.Id, PieceType.Unknown, piece.PlayerId)
            };

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