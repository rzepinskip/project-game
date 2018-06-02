using System;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.GamePlay.BasicPlayer;
using PlayerStateCoordinator.GamePlay.NormalPlayer.States;

namespace Player.StrategyGroups.Basic
{
    public class BasicStrategy : NormalPlayerStrategy
    {
        protected BasicStrategyInfo BasicStrategyInfo;

        public BasicStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) : base(player, board,
            playerGuid, gameId)
        {
            BasicStrategyInfo = new BasicStrategyInfo(board, player.Id, playerGuid, gameId, player.Team,
                NormalPlayerStrategyInfo.UndiscoveredGoalFields);
        }

        public override State GetBeginningState()
        {
            return new InitStrategyState(BasicStrategyInfo);
        }
    }
}