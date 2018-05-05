using ClientsCommon.ActionAvailability.Helpers;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityLink
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