using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace Player.StrategyGroups.Basic
{
    public class BasicStrategyGroup : StrategyGroup
    {
        public override Strategy GetStrategyFor(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> playerBases)
        {
            return new BasicStrategy(player, board, playerGuid, gameId);
        }
    }
}