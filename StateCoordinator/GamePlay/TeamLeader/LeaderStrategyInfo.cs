using System;
using Common;
using Common.BoardObjects;

namespace PlayerStateCoordinator.GamePlay.TeamLeader
{
    public class LeaderStrategyInfo : GamePlayStrategyInfo
    {
        public LeaderStrategyInfo(BoardBase board, int playerId, Guid playerGuid, int gameId, TeamColor team) : base(
            board, playerId, playerGuid, gameId, team)
        {
        }
    }
}