using System;
using System.Collections.Generic;
using System.Text;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    public class DestroyPieceActionHandler : ActionHandler
    {
        public DestroyPieceActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            var playerInfo = Board.Players[PlayerId];
            return new DestroyAvailabilityChain(PlayerId, Board.Players).ActionAvailable();
        }

        public override DataFieldSet Respond()
        {
            if(!Validate())
                return DataFieldSet.Create(PlayerId, new Piece[0]);

            var player = Board.Players[PlayerId];
            var playerPiece = player.Piece;

            player.Piece = null;
            playerPiece.Type = PieceType.Destroyed;

            return DataFieldSet.Create(PlayerId, new [] { playerPiece });
        }
    }
}
