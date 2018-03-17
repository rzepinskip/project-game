using static Shared.CommonResources;
using Shared.BoardObjects;
using Shared.ActionAvailability.ActionAvailabilityHelpers;

namespace Shared.ActionAvailability.AvailabilityLink
{
    class IsAvailableTeamAreaLink : AvailabilityLinkBase
    {
        private Location location;
        private MoveType move;
        private int goalAreaSize;
        private int taskAreaSize;
        private TeamColour team;
        public IsAvailableTeamAreaLink(Location location, MoveType move, int goalAreaSize, int taskAreaSize, TeamColour team)
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
