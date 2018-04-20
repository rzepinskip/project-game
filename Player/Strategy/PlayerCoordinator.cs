using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging.Requests;
using Player.Interfaces;
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

        public readonly GameStateInfo _gameStateInfo;
        public PlayerCoordinator(string gameName)
        {
            _gameStateInfo = new GameStateInfo(gameName);
            CurrentStrategyState = new GetGamesState(_gameStateInfo);
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

        public void NextState()
        {
            CurrentStrategyState = CurrentStrategyState.GetNextState();
        }

        public void UpdateGameStateInfo(IEnumerable<GameInfo> gameInfo)
        {
            _gameStateInfo.GameInfo = gameInfo;
        }

        public void NotifyAboutGameEnd()
        {
            _gameStateInfo.IsRunning = false;
        }

        public void CreatePlayerStrategy()
        {
            _gameStateInfo.CreatePlayerStrategy();
        }

        public void CreatePlayerStrategyFactory(IPlayerStrategyFactory playerStrategyFactory)
        {
            _gameStateInfo.PlayerStrategyFactory = playerStrategyFactory;
        }

        public BaseState CurrentStrategyState { get; set; }
    }
}
