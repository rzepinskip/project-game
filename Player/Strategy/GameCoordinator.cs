using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.States;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    class GameCoordinator : IStrategy
    {
        /// <summary>
        /// TODO: Implement visitor pattern for PlayerStrategy creation
        /// 
        /// </summary>
        private readonly GameInfo gameInfo;
        public IMessage NextMove()
        {
            throw new NotImplementedException();
        }

        public BaseState CurrentStrategyState { get; set; }
    }
}
