using ClientsCommon.ActionAvailability.Helpers;
using Common;
using Common.BoardObjects;

namespace ClientsCommon.ActionAvailability.AvailabilityLink
{
    internal class IsAvailableTeamAreaLink : AvailabilityLinkBase
    {
        private readonly int goalAreaSize;
        private readonly Location location;
        private readonly Direction move;
        private readonly int taskAreaSize;
        private readonly TeamColor team;

        public IsAvailableTeamAreaLink(Location location, Direction move, int goalAreaSize, int taskAreaSize,
            TeamColor team)
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