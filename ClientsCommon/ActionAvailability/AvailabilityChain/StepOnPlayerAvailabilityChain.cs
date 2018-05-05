using ClientsCommon.ActionAvailability.AvailabilityLink;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace ClientsCommon.ActionAvailability.AvailabilityChain
{
    public class StepOnPlayerAvailabilityChain : IAvailabilityChain
    {
        private readonly IBoard board;
        private readonly Direction direction;
        private readonly Location location;
        private readonly TeamColor team;

        public StepOnPlayerAvailabilityChain(Location location, Direction direction, TeamColor team, IBoard board)
        {
            this.location = location;
            this.direction = direction;
            this.team = team;
            this.board = board;
        }

        public bool ActionAvailable()
        {
            var builder =
                new AvailabilityChainBuilder(new IsInsideBoardLink(location, direction, board.Width, board.Height))
                    .AddNextLink(new IsAvailableTeamAreaLink(location, direction, board.GoalAreaSize,
                        board.TaskAreaSize, team))
                    .AddNextLink(new IsFieldPlayerOccupiedLink(location, direction, board));
            return builder.Build().ValidateLink();
        }
    }
}