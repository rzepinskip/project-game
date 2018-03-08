using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;
using Shared;

namespace GameMaster.ActionAvailability
{
    public static class MoveAvailability
    {
        public static Location GetNewLocation(Location l, CommonResources.MoveType direction)
        {
            Location nl = new Location(){ X = l.X, Y = l.Y };
            switch (direction)
            {
                case CommonResources.MoveType.Down:
                    nl.Y = l.Y - 1;
                    break;
                case CommonResources.MoveType.Left:
                    nl.X = l.X - 1;
                    break;
                case CommonResources.MoveType.Right:
                    nl.X = l.X + 1;
                    break;
                case CommonResources.MoveType.Up:
                    nl.Y = l.Y + 1;
                    break;
            }
            return nl;
        }
        public static bool IsInsideBoard(Location l, CommonResources.MoveType direction, int BoardWidth, int BoardHeight)
        {
            bool response = true;
            Location nl = GetNewLocation(l, direction);
            if (nl.Y < 0 || nl.X < 0 || nl.X > BoardWidth - 1 || nl.Y > BoardHeight - 1)
                response = false;
            return response;
        }
        public static bool IsAvailableTeamArea(Location l, CommonResources.Team team, CommonResources.MoveType direction, int GoalAreaSize, int TaskAreaSize )
        {
            bool response = true;
            if (team == CommonResources.Team.Red)
            {
                if(GetNewLocation(l, direction).Y > TaskAreaSize + GoalAreaSize - 1)
                {
                    response = false;
                }
            }
            else
            {
                if(GetNewLocation(l, direction).Y < GoalAreaSize)
                {
                    response = false;
                }
            }
            return response;

        }
        public static bool IsFieldPlayerUnoccupied(Location l, CommonResources.MoveType direction, Board board)
        {
            bool response = true;
            Location nl = GetNewLocation(l, direction);
            if (board.Content[nl.X, nl.Y].PlayerId != null)
                response = false;
            return response;
        }
    }
}
