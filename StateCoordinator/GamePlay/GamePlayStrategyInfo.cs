using System;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common;

namespace PlayerStateCoordinator.GamePlay
{
    public abstract class GamePlayStrategyInfo : BaseInfo
    {
        public readonly BoardBase Board;
        public readonly int GameId;
        public readonly Guid PlayerGuid;
        public readonly int PlayerId;
        public readonly TeamColor Team;
        public Location TargetLocation;

        protected GamePlayStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team)
        {
            Board = board;
            PlayerId = playerId;
            PlayerGuid = playerGuid;
            GameId = gameId;
            Team = team;
        }

        public Location CurrentLocation => Board.Players[PlayerId].Location;
    }
}