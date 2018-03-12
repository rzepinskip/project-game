using static Shared.CommonResources;
using Shared.BoardObjects;
using Shared.ActionAvailability.ActionAvailabilityHelpers;

namespace Shared.ActionAvailability.AvailabilityLink
{
    class IsInsideBoardLink : AvailabilityLinkBase
    {
        private Location location;
        private MoveType move;
        private int width;
        private int height;

        public IsInsideBoardLink(Location location, MoveType move, int width, int height)
        {
            this.location = location;
            this.move = move;
            this.width = width;
            this.height = height;
        }
        protected override bool Validate()
        {
            return MoveAvailability.IsInsideBoard(location, move, width, height);
        }
    }
}
