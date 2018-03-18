using System;
using System.Xml.Serialization;
using Shared.BoardObjects;
using Shared.ResponseMessages;

namespace Shared.GameMessages
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class PickUpPiece : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            var response = new PickUpPieceResponse();

            var player = board.Players[PlayerId];
            if (!board.IsLocationInTaskArea(player.Location))
                return response;

            var playerField = board.Content[player.Location.X, player.Location.Y] as TaskField;

            if (!playerField.PieceId.HasValue)
                return response;

            var piece = board.Pieces[playerField.PieceId.Value];
            piece.PlayerId = PlayerId;

            player.Piece = piece;
            playerField.PieceId = null;

            response = new PickUpPieceResponse()
            {
                PlayerId = this.PlayerId,
                Piece = new Piece()
                {
                    Id = piece.Id,
                    PlayerId = piece.PlayerId,
                    Type = CommonResources.PieceType.Unknown
                }
            };
            
            return response;
        }
        
        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, CommonResources.ActionType.PickUp);
        }

        public override double GetDelay(ActionCosts actionCosts)
        {
            return actionCosts.PickUpDelay;
        }
    }

}
