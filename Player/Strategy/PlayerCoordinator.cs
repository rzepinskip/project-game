using System.Collections.Generic;
using Common;
using Common.Interfaces;
using Player.Interfaces;
using Player.Strategy.StateInfo;
using Player.Strategy.States;
using Player.Strategy.States.GameStates;

namespace Player.Strategy
{
    internal class PlayerCoordinator : IStrategy
    {
        /// <summary>
        ///     TODO: Implement visitor pattern for PlayerStrategy creation
        /// </summary>
        public readonly GameStateInfo _gameStateInfo;

        public PlayerCoordinator(string gameName, TeamColor color, PlayerType role)
        {
            _gameStateInfo = new GameStateInfo(gameName, color, role);
            CurrentStrategyState = new GetGamesState(_gameStateInfo);
        }

        public IMessage NextMove()
        {
            var message = CurrentStrategyState.GetNextMessage();
            //CurrentStrategyState = CurrentStrategyState.GetNextState();

            return message;
        }

        public bool StrategyReturnsMessage()
        {
            return CurrentStrategyState.StateReturnsMessage();
        }

        public BaseState CurrentStrategyState { get; set; }

        public void UpdateJoinInfo(bool info)
        {
            _gameStateInfo.JoiningSuccessful = info;
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
            _gameStateInfo.CreatePlayerStrategy();
        }
    }
}