using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;

namespace Player.StrategyGroups
{
    public abstract class StrategyGroup
    {
        public abstract Strategy Create(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> players);
    }
}