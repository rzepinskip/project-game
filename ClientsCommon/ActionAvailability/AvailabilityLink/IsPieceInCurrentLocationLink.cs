using ClientsCommon.ActionAvailability.Helpers;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityLink
{
    internal class IsPieceInCurrentLocationLink : AvailabilityLinkBase
    {
        private readonly IBoard board;
        private readonly Location location;

        public IsPieceInCurrentLocationLink(Location location, IBoard board)
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