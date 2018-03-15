using Shared.BoardObjects;
using Shared.ResponseMessages;
using System.Xml.Serialization;

namespace Shared.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PlacePiece : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            var player = board.Players[PlayerId];

            if (player.Piece.Type == CommonResources.PieceType.Sham)
                return new PlacePieceResponse();

            var playerGoalField = board.Content[player.Location.X, player.Location.Y];
            var response = new PlacePieceResponse()
            {
                GoalField = playerGoalField as GoalField
            };
            
            return response;
        }
        
        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, CommonResources.ActionType.Place);
        }
    }

}
