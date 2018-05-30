using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace Player.StrategyGroups
{
    public abstract class StrategyGroup
    {
        public abstract Strategy GetStrategyFor(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> players);
    }
}