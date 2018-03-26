using System.Xml.Serialization;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPieceRequest : Request
    {
        public PickUpPieceRequest(int playerId) : base(playerId)
        {
        }

        public override Response Execute(IGameMasterBoard board)
        {
            var response = new PickUpPieceResponse(PlayerId);

            var player = board.Players[PlayerId];

            var actionAvailibility = new PickUpAvailabilityChain(player.Location, board, PlayerId);

            if (actionAvailibility.ActionAvailable())
            {
                var playerField = board[player.Location] as TaskField;

                var piece = board.Pieces[playerField.PieceId.Value];
                piece.PlayerId = PlayerId;

                player.Piece = piece;
                playerField.PieceId = null;

                response = new PickUpPieceResponse(PlayerId, new Piece(piece.Id, PieceType.Unknown, piece.PlayerId));
            }

            return response;
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.PickUp, base.ToLog());
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PickUpDelay;
        }
    }
}