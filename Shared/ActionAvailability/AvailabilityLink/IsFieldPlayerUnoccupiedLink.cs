using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using static Shared.CommonResources;

namespace Shared.ActionAvailability.AvailabilityLink
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
            return new MoveAvailability().IsFieldPlayerUnoccupied(location, move, board);
        }
    }
}
