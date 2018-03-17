using Shared.BoardObjects;
using Shared.GameMessages;
using System;
using System.Collections.Generic;
using System.Text;
using static Player.Strategy.PlayerStrategy;

namespace Player.Strategy.StateTransition
{
    abstract class BaseTransition
    {
        public abstract GameMessage ExecuteStrategy(Board board, out PlayerState changedState);
    }
}
