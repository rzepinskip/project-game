using Common.ActionAvailability.Helpers;
using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.AvailabilityLink
{
    internal class IsFieldPlayerOccupiedLink : AvailabilityLinkBase
    {
        private readonly IBoard board;
        private readonly Location location;
        private readonly Direction move;

        public IsFieldPlayerOccupiedLink(Location location, Direction move, IBoard board)
        {
            this.location = location;
            this.move = move;
            this.board = board;
        }

        protected override bool Validate()
        {
            return ! new MoveAvailability().IsFieldPlayerUnoccupied(location, move, board);
        }
    }
}