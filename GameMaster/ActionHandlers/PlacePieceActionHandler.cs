using System;
using ClientsCommon.ActionAvailability.AvailabilityChain;
using Common;
using Common.BoardObjects;

namespace GameMaster.ActionHandlers
{
    internal class PlacePieceActionHandler : ActionHandler
    {
        public PlacePieceActionHandler(int playerId, GameMasterBoard board) : base(playerId, board)
        {
            PlayerId = playerId;
            Board = board;
        }

        protected override bool Validate()
        {
            var playerInfo = Board.Players[PlayerId];
            return new PlaceAvailabilityChain(playerInfo.Location, Board, PlayerId).ActionAvailable();
        }

        public override DataFieldSet Respond()
        {
            ///TODO: different action on TaskField
            if (!Validate())
                return DataFieldSet.Create(PlayerId, new GoalField[0]);
            var player = Board.Players[PlayerId];
            var piece = player.Piece;

            player.Piece = null;

            if (piece.Type == PieceType.Sham)
                return DataFieldSet.Create(PlayerId, new GoalField[0]);

            var playerGoalField = Board[player.Location] as GoalField;

            if (playerGoalField != null && playerGoalField.Type == GoalFieldType.Goal)
                Board.MarkGoalAsCompleted(playerGoalField);

            return DataFieldSet.Create(PlayerId, new[] {playerGoalField});
        }
    }
}