using System;
using System.Collections.Generic;
using Common;
using Common.ActionAvailability.AvailabilityChain;
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
            var playerInfo = Board.Players[PlayerId];
            return new PickUpAvailabilityChain(playerInfo.Location, Board, PlayerId).ActionAvailable();
        }

        public override DataFieldSet Respond()
        {
            if (!Validate())
                return DataFieldSet.Create(PlayerId,  new Piece[0]);

            var player = Board.Players[PlayerId];
            var playerField = Board[player.Location] as TaskField;

            var piece = Board.Pieces[playerField.PieceId.Value];
            piece.PlayerId = PlayerId;

            player.Piece = piece;
            playerField.PieceId = null;

            return DataFieldSet.Create(PlayerId, new[] { new Piece(piece.Id, PieceType.Unknown, piece.PlayerId) });
        }
    }
}