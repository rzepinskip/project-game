using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common.States;

namespace Player
{
    public abstract class Strategy
    {

        protected Strategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId)
        {
        }

        public abstract State GetBeginningState();

        public static Strategy Create(PlayerBase player, BoardBase board, Guid playerGuid, int gameId,
            IEnumerable<PlayerBase> players)
        {
            switch (player.Role)
            {
                case PlayerType.Member:
                    return new NormalPlayerStrategy(player, board, playerGuid, gameId);
                case PlayerType.Leader:
                    return new LeaderStrategy(player, board, playerGuid, gameId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(player.Role), player.Role, null);
            }
        }
    }
}