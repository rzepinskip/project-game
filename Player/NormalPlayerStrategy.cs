using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common.States;
using PlayerStateCoordinator.NormalPlayer.States;

namespace Player
{
    public class NormalPlayerStrategy : Strategy
    {

        public NormalPlayerStrategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId) :base(player, board, playerGuid, gameId)
        {
            BeginningState = new InitStrategyState(GameStrategyInfo);
        }

        public override State GetBeginningState()
        {
            return BeginningState;
        }
    }
}
