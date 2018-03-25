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
    public class PlacePieceRequest : Request
    {
        public PlacePieceRequest(int playerId) : base(playerId)
        {
        }

        public override Response Execute(IGameMasterBoard board)
        {
            var response = new PlacePieceResponse(PlayerId);
            ///TODO: different action on TaskField
            var player = board.Players[PlayerId];
            var piece = player.Piece;

            var actionAvailibility = new PlaceAvailabilityChain(player.Location, board, PlayerId);
            if (actionAvailibility.ActionAvailable())
            {
                player.Piece = null;
                if (piece.Type == PieceType.Sham)
                    return new PlacePieceResponse(PlayerId);

                var playerGoalField = board[player.Location] as GoalField;


                ///TODO: GameMaster counter
                if (playerGoalField != null && playerGoalField.Type == GoalFieldType.Goal)
                {
                    board.MarkGoalAsCompleted(playerGoalField);
                }

                response = new PlacePieceResponse(PlayerId, playerGoalField);
            }

            return response;
        }

        public override string ToLog()
        {
            return string.Join(',', ActionType.Place, base.ToLog());
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PlacingDelay;
        }
    }
}