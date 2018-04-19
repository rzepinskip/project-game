using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;
using Player.Strategy.States.StrategyStates;

namespace Player.Strategy
{
    class PlayerCoordinator : IStrategy
    {
        /// <summary>
        /// TODO: Implement visitor pattern for PlayerStrategy creation
        /// 
        /// </summary>

        private readonly GameStateInfo _gameStateInfo;
        public PlayerCoordinator()
        {
            this._gameStateInfo = new GameStateInfo();
        }

        public void UpdateJoinInfo(bool info)
        {
            _gameStateInfo.JoiningSuccessful = info;
        }
        
        public IMessage NextMove()
        {
            var message = this.CurrentStrategyState.GetNextMessage();
            this.CurrentStrategyState = this.CurrentStrategyState.GetNextState();

            return message;
        }

        public void UpdateGameStateInfo(IEnumerable<GameInfo> gameInfo)
        {
            _gameStateInfo.GameInfo = gameInfo;
        }

        public BaseState CurrentStrategyState { get; set; }
    }
}
