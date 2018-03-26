using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class PickUpActionHandler : ActionHandler
    {
        public PickUpActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            throw new NotImplementedException();
        }

        public override DataFieldSet Respond()
        {
            var player = Board.Players[PlayerId];
            if (!Board.IsLocationInTaskArea(player.Location))
                return DataFieldSet.CreateMoveDataSet(PlayerId,  new Piece[0]);

            var playerField = Board[player.Location] as TaskField;

            if (!playerField.PieceId.HasValue)
                return DataFieldSet.CreateMoveDataSet(PlayerId, new Piece[0]);

            var piece = Board.Pieces[playerField.PieceId.Value];
            piece.PlayerId = PlayerId;

            player.Piece = piece;
            playerField.PieceId = null;

            return DataFieldSet.CreateMoveDataSet(PlayerId, new[] { new Piece(piece.Id, PieceType.Unknown, piece.PlayerId) });
        }
    }
}