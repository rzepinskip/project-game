using System;
using Common;

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
            throw new NotImplementedException();
        }

        public override DataFieldSet Respond()
        {
            var player = Board.Players[PlayerId];
            var playerPiece = player.Piece;

            if (playerPiece.Type == PieceType.Sham)
                player.Piece = null;

            return DataFieldSet.CreateMoveDataSet(PlayerId, new [] { playerPiece });
        }
    }
}