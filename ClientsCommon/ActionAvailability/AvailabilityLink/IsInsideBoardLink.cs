using ClientsCommon.ActionAvailability.Helpers;
using Common;
using Common.BoardObjects;

namespace ClientsCommon.ActionAvailability.AvailabilityLink
{
    internal class IsInsideBoardLink : AvailabilityLinkBase
    {
        private readonly int height;
        private readonly Location location;
        private readonly Direction move;
        private readonly int width;

        public IsInsideBoardLink(Location location, Direction move, int width, int height)
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