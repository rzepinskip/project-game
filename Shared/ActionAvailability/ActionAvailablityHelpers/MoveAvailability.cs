using Shared.BoardObjects;
using Shared;

namespace Shared.ActionAvailability.ActionAvailabilityHelpers
{
    public class MoveAvailability
    {
        public bool IsInsideBoard(Location l, CommonResources.MoveType direction, int BoardWidth, int BoardHeight)
        {
            var response = true;
            var nl = l.GetNewLocation(direction);
            if (nl.Y < 0 || nl.X < 0 || nl.X > BoardWidth - 1 || nl.Y > BoardHeight - 1)
                response = false;
            return response;
        }
        public bool IsAvailableTeamArea(Location l, CommonResources.TeamColour team, CommonResources.MoveType direction, int GoalAreaSize, int TaskAreaSize )
        {
            var response = true;
            if (team == CommonResources.TeamColour.Red)
            {
                if(l.GetNewLocation(direction).Y > TaskAreaSize + GoalAreaSize - 1)
                {
                    response = false;
                }
            }
            else
            {
                if(l.GetNewLocation(direction).Y < GoalAreaSize)
                {
                    response = false;
                }
            }
            return response;

        }
        public bool IsFieldPlayerUnoccupied(Location l, CommonResources.MoveType direction, Board board)
        {
            var response = true;
            var nl = l.GetNewLocation(direction);
            if (board.Content[nl.X, nl.Y].PlayerId != null)
                response = false;
            return response;
        }
    }
}
