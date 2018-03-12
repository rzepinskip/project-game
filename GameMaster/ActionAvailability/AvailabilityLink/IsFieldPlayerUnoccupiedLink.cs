using GameMaster.ActionAvailability.ActionAvailabilityHelpers;
using Shared.Board;
using static Shared.CommonResources;

namespace GameMaster.ActionAvailability.AvailabilityLink
{
    class IsFieldPlayerUnoccupiedLink : AvailabilityLinkBase
    {
        private Location location;
        private MoveType move;
        private Board board;
        public IsFieldPlayerUnoccupiedLink(Location location, MoveType move, Board board)
        {
            this.location = location;
            this.move = move;
            this.board = board;
        }
        protected override bool Validate()
        {
            return MoveAvailability.IsFieldPlayerUnoccupied(location, move, board);
        }
    }
}
