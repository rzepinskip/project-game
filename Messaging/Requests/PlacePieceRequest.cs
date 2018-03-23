using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Logging;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PlacePieceRequest : Request
    {
        public PlacePieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        public override ActionInfo GetActionInfo()
        {
            return new PlaceActionInfo(PlayerGuid);
        }

        /*
        TODO: Move commented methods to GameMaster.Execute(PlacePieceInfo, ...)
        public override Response Execute(IBoard board)
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
            if (goalField != null) goalField.Type = GoalFieldType.NonGoal;

            var response = new PlacePieceResponse(PlayerId, playerGoalField as GoalField);

            return response;
        }

        */
        public override string ToLog()
        {
            return string.Join(',', ActionType.Place, base.ToLog());
        }
    }
}