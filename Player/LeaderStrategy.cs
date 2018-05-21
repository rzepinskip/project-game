using System;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.TeamLeader;
using PlayerStateCoordinator.TeamLeader.States;

namespace Player
{
    public class LeaderStrategy : Strategy
    {
        protected LeaderStrategyState BeginningState;
        protected LeaderStrategyInfo LeaderStrategyInfo;
        public LeaderStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) : base(player, board, playerGuid, gameId)
        {
            LeaderStrategyInfo = new LeaderStrategyInfo(board, player.Id, playerGuid, gameId, player.Team);

        }

        public override State GetBeginningState()
        {
            return  new InitLeaderStrategyState(LeaderStrategyInfo);
        }
    }
}