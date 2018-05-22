﻿using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Common.Interfaces;

namespace Player.StrategyGroup
{
    internal class PrimitiveStrategyGroup : StrategyGroup
    {
        public override Strategy Create(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> playerBases)
        {
            return new NormalPlayerStrategy(player, board, playerGuid, gameId);
        }
    }
}