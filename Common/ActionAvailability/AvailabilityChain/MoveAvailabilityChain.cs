using Common.ActionAvailability.AvailabilityLink;
using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.AvailabilityChain
{
    public class MoveAvailabilityChain : IAvailabilityChain
    {
        private readonly IBoard board;
        private readonly Direction direction;
        private readonly Location location;
        private readonly TeamColor team;

        public MoveAvailabilityChain(Location location, Direction direction, TeamColor team, IBoard board)
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
                    .AddNextLink(new IsFieldPlayerUnoccupiedLink(location, direction, board));
            return builder.Build().ValidateLink();
        }
    }
}