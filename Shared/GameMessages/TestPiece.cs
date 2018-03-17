﻿using System.Xml.Serialization;
using Shared.BoardObjects;
using Shared.ResponseMessages;

namespace Shared.GameMessages.PieceActions
{
    [XmlRoot(Namespace = "https://se2.mini.pw.edu.pl/17-results/")]
    public class TestPiece : GameMessage
    {
        public override ResponseMessage Execute(Board board)
        {
            var playerPiece = board.Players[PlayerId].Piece;

            var response = new TestPieceResponse()
            {
                PlayerId = this.PlayerId,
                Piece = playerPiece
            };

            return response;
        }
        
        public override ActionLog ToLog(int playerId, PlayerInfo playerInfo)
        {
            return new ActionLog(playerId, GameId, PlayerGuid, playerInfo, CommonResources.ActionType.Test);
        }
    }
}