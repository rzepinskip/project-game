using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;

namespace Shared.ActionAvailability.AvailabilityLink
{
    internal class IsPieceInCurrentLocationLink : AvailabilityLinkBase
    {
        private readonly Board board;
        private readonly Location location;

        public IsPieceInCurrentLocationLink(Location location, Board board)
        {
            this.location = location;
            this.board = board;
        }

        protected override bool Validate()
        {
            return new PieceRelatedAvailability().IsPieceInCurrentLocation(location, board);
        }
    }
}