using Shared.Board;
using static Shared.CommonResources;
using GameMaster.ActionAvailability.AvailabilityLink;

namespace GameMaster.ActionAvailability.AvailabilityChain
{
    public class MoveAvailabilityChain :IAvailabilityChain
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

        public bool ActionAvailable() 
        {
            AvailabilityChainBuilder builder = new AvailabilityChainBuilder(new IsInsideBoardLink(location, direction, board.Width, board.Height))
                .AddNextLink(new IsAvailableTeamAreaLink(location, direction, board.GoalAreaSize, board.TaskAreaSize, team))
                .AddNextLink(new IsFieldPlayerUnoccupiedLink(location, direction, board));
            return builder.Build().ValidateLink();
        }
        
    }
}
