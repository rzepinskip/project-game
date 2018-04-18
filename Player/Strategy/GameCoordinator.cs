using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    class GameCoordinator : IStrategy
    {
        private readonly GameInfo gameInfo;
        public Request NextMove(Location location)
        {
            throw new NotImplementedException();
        }

        public StrategyState CurrentStrategyState { get; set; }
    }
}
