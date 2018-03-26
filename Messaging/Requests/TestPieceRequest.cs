using System.Xml.Serialization;
using Common;
using Common.ActionInfo;
using Common.Interfaces;

namespace Messaging.Requests
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class TestPieceRequest : Request
    {
        public TestPieceRequest(string playerGuid) : base(playerGuid)
        {
        }

        /*
        TODO: move to GameMaster.ExecuteAction(TestPieceActionInfo ...)
        public override Response Execute(IBoard board)
        {
            var player = board.Players[PlayerId];
            var playerPiece = player.Piece;

            if (playerPiece.Type == PieceType.Sham)
                player.Piece = null;

            var response = new TestPieceResponse(PlayerId, playerPiece);

            return response;
        }

        */
        public override string ToLog()
        {
            return string.Join(',', ActionType.Test, base.ToLog());
        }

        public override ActionInfo GetActionInfo()
        {
            return new TestActionInfo(PlayerGuid);
        }
    }
}