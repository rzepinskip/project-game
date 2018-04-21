using System;
using Common;
using Common.ActionAvailability.AvailabilityChain;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class TestPieceActionHandler : ActionHandler
    {
        public TestPieceActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            return new TestAvailabilityChain(PlayerId, Board.Players).ActionAvailable();
        }

        public override DataFieldSet Respond()
        {
            if (!Validate())
                return  DataFieldSet.Create(PlayerId, new Piece[0]);

            var player = Board.Players[PlayerId];
            var playerPiece = player.Piece;

            if (playerPiece.Type == PieceType.Sham)
                player.Piece = null;

            return DataFieldSet.Create(PlayerId, new [] { playerPiece });
        }
    }
}