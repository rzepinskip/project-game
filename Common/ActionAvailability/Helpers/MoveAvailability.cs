using Common.BoardObjects;
using Common.Interfaces;

namespace Common.ActionAvailability.Helpers
{
    public class MoveAvailability
    {
        public bool IsInsideBoard(Location l, Direction direction, int BoardWidth, int BoardHeight)
        {
            var response = true;
            var nl = l.GetNewLocation(direction);
            if (nl.Y < 0 || nl.X < 0 || nl.X > BoardWidth - 1 || nl.Y > BoardHeight - 1)
                response = false;
            return response;
        }

        public bool IsAvailableTeamArea(Location l, TeamColor team, Direction direction,
            int GoalAreaSize, int TaskAreaSize)
        {
            var response = true;
            if (team == TeamColor.Blue)
            {
                if (l.GetNewLocation(direction).Y > TaskAreaSize + GoalAreaSize - 1) response = false;
            }
            else
            {
                if (l.GetNewLocation(direction).Y < GoalAreaSize) response = false;
            }

            return response;
        }

        public bool IsFieldPlayerUnoccupied(Location l, Direction direction, IBoard board)
        {
            var response = true;
            var nl = l.GetNewLocation(direction);
            if (board[nl].PlayerId != null)
                response = false;
            return response;
        }
    }
}