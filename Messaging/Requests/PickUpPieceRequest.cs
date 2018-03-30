using System.Xml.Serialization;
using Common;
using Common.ActionInfo;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPieceRequest : Request
    {
        public PickUpPieceRequest(string playerGuid) : base(playerGuid)
        {
        }


        /*
        //TODO: move to GameMaster.ExecuteAction(PickUpPieceActionInfor, ...)

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
        */

        public override string ToLog()
        {
            return string.Join(',', ActionType.PickUp, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new PickUpActionInfo(PlayerGuid);
        }
    }
}