using System.Xml.Serialization;
using Common;
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
            ///TODO: different action on TaskField
            var player = board.Players[PlayerId];
            var piece = player.Piece;

            player.Piece = null;

            if (piece.Type == PieceType.Sham)
                return new PlacePieceResponse(PlayerId);

            var playerGoalField = board[player.Location];
            var goalField = playerGoalField as GoalField;


            ///TODO: GameMaster counter
            if (goalField != null)
            {
                board.MarkGoalAsCompleted(goalField);
            }

            var response = new PlacePieceResponse(PlayerId, playerGoalField as GoalField);

            return response;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.Place);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PlacingDelay;
        }
    }
}