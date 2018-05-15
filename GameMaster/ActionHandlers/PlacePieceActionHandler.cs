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

        public override BoardData Respond()
        {
            //TODO: different action on TaskField

            if (!Validate())
                return BoardData.Create(PlayerId, new GoalField[0]);

            var player = Board.Players[PlayerId];
            var piece = player.Piece;

            player.Piece = null;
            Board.Pieces.Remove(piece.Id);

            if (piece.Type == PieceType.Sham)
                return null;

            var playerGoalField = Board[player.Location] as GoalField;

            if (playerGoalField != null && playerGoalField.Type == GoalFieldType.Goal)
                Board.MarkGoalAsCompleted(playerGoalField);

            return BoardData.Create(PlayerId, new[] { playerGoalField });
        }
    }
}