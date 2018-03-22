using Shared.ActionAvailability.ActionAvailabilityHelpers;
using Shared.BoardObjects;
using static Shared.CommonResources;

namespace Shared.ActionAvailability.AvailabilityLink
{
    internal class IsAvailableTeamAreaLink : AvailabilityLinkBase
    {
        private readonly int goalAreaSize;
        private readonly Location location;
        private readonly MoveType move;
        private readonly int taskAreaSize;
        private readonly TeamColour team;

        public IsAvailableTeamAreaLink(Location location, MoveType move, int goalAreaSize, int taskAreaSize,
            TeamColour team)
        {
            this.location = location;
            this.move = move;
            this.taskAreaSize = taskAreaSize;
            this.goalAreaSize = goalAreaSize;
            this.team = team;
        }

        protected override bool Validate()
        {
            return new MoveAvailability().IsAvailableTeamArea(location, team, move, goalAreaSize, taskAreaSize);
        }
    }
}