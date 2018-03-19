using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using static Shared.CommonResources;

namespace Shared.ActionAvailability.AvailabilityLink
{
    internal class IsFieldPlayerUnoccupiedLink : AvailabilityLinkBase
    {
        private readonly Board board;
        private readonly Location location;
        private readonly MoveType move;

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