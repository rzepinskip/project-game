using Common.ActionAvailability.ActionAvailabilityHelpers;
using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.AvailabilityLink
{
    internal class IsFieldPlayerUnoccupiedLink : AvailabilityLinkBase
    {
        private readonly IBoard board;
        private readonly Location location;
        private readonly Direction move;

        public IsFieldPlayerUnoccupiedLink(Location location, Direction move, IBoard board)
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