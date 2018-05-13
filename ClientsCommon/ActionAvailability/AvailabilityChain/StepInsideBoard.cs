using ClientsCommon.ActionAvailability.AvailabilityLink;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    public class StepInsideBoard : IAvailabilityChain
    {
        private readonly IBoard board;
        private readonly Direction direction;
        private readonly Location location;
        private readonly TeamColor team;

        public StepInsideBoard(Location location, Direction direction, TeamColor team, IBoard board)
        {
            this.location = location;
            this.direction = direction;
            this.team = team;
            this.board = board;
        }

        public bool ActionAvailable()
        {
            var builder = new AvailabilityChainBuilder(new IsInsideBoardLink(location, direction, board.Width, board.Height));
            return builder.Build().ValidateLink();
        }
    }
}