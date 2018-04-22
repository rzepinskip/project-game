using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Player.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy
{
    class PlayerCoordinator : IStrategy
    {
        /// <summary>
        /// TODO: Implement visitor pattern for PlayerStrategy creation
        /// 
        /// </summary>

        public readonly GameStateInfo _gameStateInfo;
        public PlayerCoordinator(string gameName, TeamColor color, PlayerType role)
        {
            _gameStateInfo = new GameStateInfo(gameName, color, role);
            CurrentStrategyState = new GetGamesState(_gameStateInfo);
        }

        public void UpdateJoinInfo(bool info)
        {
            _gameStateInfo.JoiningSuccessful = info;
        }
        
        public IMessage NextMove()
        {
            var message = this.CurrentStrategyState.GetNextMessage();
            //CurrentStrategyState = CurrentStrategyState.GetNextState();

            return message;
        }

        public void NextState()
        {
            CurrentStrategyState = CurrentStrategyState.GetNextState();
        }

        public bool StrategyReturnsMessage()
        {
            return CurrentStrategyState.StateReturnsMessage();
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
            _gameStateInfo.CreatePlayerStrategy();
        }

        public BaseState CurrentStrategyState { get; set; }
        
    }
}
