using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.BoardObjects;

namespace Player.StrategyGroup
{
    internal class AdvancedStrategyGroup : StrategyGroup
    {
        public override Strategy Create(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> players)
        {
            if (player.Role == PlayerType.Member || players.Count <= 2)
            {
                return new NormalPlayerStrategy(player, board, playerGuid, gameId);
            }
            return new LeaderStrategy(player, board, playerGuid, gameId);
            
        }
    }
}