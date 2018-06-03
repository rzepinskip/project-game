using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using Player.StrategyGroups.Basic;

namespace Player.StrategyGroups.Advanced
{
    public class AdvancedStrategyGroup : StrategyGroup
    {
        public override Strategy GetStrategyFor(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            List<PlayerBase> players)
        {
            if (player.Role == PlayerType.Member || players.Count <= 2)
                return new NormalPlayerStrategy(player, board, playerGuid, gameId);

            return new LeaderStrategy(player, board, playerGuid, gameId);
        }
    }
}