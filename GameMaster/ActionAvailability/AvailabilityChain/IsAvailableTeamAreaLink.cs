using System;
using System.Collections.Generic;
using System.Text;
using static Shared.CommonResources;
using Shared.Board;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    class IsAvailableTeamAreaLink : AvailabilityChainBase
    {
        private Location location;
        private MoveType move;
        private int goalAreaSize;
        private int taskAreaSize;
        private Team team;
        public IsAvailableTeamAreaLink(Location location, MoveType move, int goalAreaSize, int taskAreaSize, Team team)
        {
            this.location = location;
            this.move = move;
            this.taskAreaSize = taskAreaSize;
            this.goalAreaSize = goalAreaSize;
            this.team = team;
        }
        protected override bool Validate()
        {
            return MoveAvailability.IsAvailableTeamArea(location, team, move, goalAreaSize, taskAreaSize);
        }
    }
}
