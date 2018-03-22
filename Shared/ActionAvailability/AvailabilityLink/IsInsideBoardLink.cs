using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using static Shared.CommonResources;

namespace Shared.ActionAvailability.AvailabilityLink
{
    internal class IsInsideBoardLink : AvailabilityLinkBase
    {
        private readonly int height;
        private readonly Location location;
        private readonly MoveType move;
        private readonly int width;

        public IsInsideBoardLink(Location location, MoveType move, int width, int height)
        {
            this.location = location;
            this.move = move;
            this.width = width;
            this.height = height;
        }

        protected override bool Validate()
        {
            return new MoveAvailability().IsInsideBoard(location, move, width, height);
        }
    }
}