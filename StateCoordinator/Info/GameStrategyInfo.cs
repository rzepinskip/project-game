using System;
using Common;
using System.Collections.Generic;
using Common.BoardObjects;

namespace PlayerStateCoordinator.Info
{
    public class GameStrategyInfo : BaseInfo
    {
        public GameStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team, List<GoalField> undiscoveredGoalFields, Location targetLocation)
        {
            Board = board;
            PlayerId = playerId;
            PlayerGuid = playerGuid;
            GameId = gameId;
            Team = team;
            UndiscoveredGoalFields = undiscoveredGoalFields;
            TargetLocation = targetLocation;
        }

        public readonly BoardBase Board;
        public readonly int PlayerId;
        public readonly Guid PlayerGuid;
        public readonly int GameId;
        public readonly TeamColor Team;
        public readonly List<GoalField> UndiscoveredGoalFields;
        public Location TargetLocation;

        public Location CurrentLocation => Board.Players[PlayerId].Location;
    }
}
