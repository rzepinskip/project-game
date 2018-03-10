using Shared.Board;
using System;
using System.Collections.Generic;
using System.Text;
using static Shared.CommonResources;
using Shared;
using GameMaster.ActionAvailability.AvailabilityChain;
namespace GameMaster.ActionAvailability
{
    public class MoveAvailabilityChain
    {

        Location location;
        MoveType direction;
        Team team;
        Board board;

        public MoveAvailabilityChain(Location location, MoveType direction, Team team, Board board) 
        {
            this.location = location;
            this.direction = direction;
            this.team = team;
            this.board = board;
        }

        public bool MoveAvailable() 
        {
            AvailabilityChainBuilder builder = new AvailabilityChainBuilder(new IsInsideBoardLink(location, direction, board.Width, board.Height))
                .AddNextLink(new IsAvailableTeamAreaLink(location, direction, board.GoalAreaSize, board.TaskAreaSize, team))
                .AddNextLink(new IsFieldPlayerUnoccupiedLink(location, direction, board));
            return builder.Build().ValidateLink();
        }
        
    }
}
