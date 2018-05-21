using System;
using System.Collections.Generic;
using Common;
using Common.BoardObjects;
using PlayerStateCoordinator.Common;
using PlayerStateCoordinator.Common.States;

namespace Player
{
    public abstract class Strategy
    {

        protected Strategy(PlayerBase player, BoardBase board, Guid playerGuid, int gameId)
        {

        }

        public abstract State GetBeginningState();
    }
}