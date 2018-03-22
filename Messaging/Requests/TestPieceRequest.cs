using System.Xml.Serialization;
using Common.Interfaces;
using Messaging.ActionHelpers;
using Messaging.Responses;

namespace Common.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class TestPieceRequest : Request
    {
        public override Response Execute(IBoard board)
        {
            var player = board.Players[PlayerId];
            var playerPiece = player.Piece;

            if (playerPiece.Type == PieceType.Sham)
                player.Piece = null;

            var response = new TestPieceResponse
            {
                PlayerId = PlayerId,
                Piece = playerPiece
            };

            return response;
        }

        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, ActionType.Test);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.TestDelay;
        }
    }
}