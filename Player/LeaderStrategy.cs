using System;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.TeamLeader.States;

namespace Player
{
    public class LeaderStrategy : Strategy
    {
        public LeaderStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) : base(player, board, playerGuid, gameId)
        {
        }

        public override State GetBeginningState()
        {
            return  new InitLeaderStrategyState(GameStrategyInfo);
        }
    }
}