using System.Xml.Serialization;
using Shared.BoardObjects;
using Shared.ResponseMessages;

namespace Shared.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class TestPiece : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            var player = board.Players[PlayerId];
            var playerPiece = player.Piece;

            if (playerPiece.Type == CommonResources.PieceType.Sham)
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
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, CommonResources.ActionType.Test);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.TestDelay;
        }
    }
}