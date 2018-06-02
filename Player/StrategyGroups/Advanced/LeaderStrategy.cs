using System;
using Common;
using Common.BoardObjects;
using Player.StrategyGroups.Basic;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;
using PlayerStateCoordinator.GamePlay.TeamLeader;

namespace Player.StrategyGroups.Advanced
{
    public class LeaderStrategy : NormalPlayerStrategy
    {
        protected LeaderStrategyInfo LeaderStrategyInfo;

        public LeaderStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) : base(player, board,
            playerGuid, gameId)
        {
            LeaderStrategyInfo = new LeaderStrategyInfo(board, player.Id, playerGuid, gameId, player.Team,
                NormalPlayerStrategyInfo.UndiscoveredGoalFields);
        }

        public override State GetBeginningState()
        {
            return new InitStrategyState(LeaderStrategyInfo);
        }
    }
}