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
            CurrentGameState = new GetGamesState(_gameStateInfo);
        }

        public IMessage NextMove()
        {
            var message = CurrentGameState.GetNextMessage();

            return message;
        }

        public bool StrategyReturnsMessage()
        {
            return CurrentGameState.StateReturnsMessage();
        }

        public BaseState CurrentGameState { get; set; }

        public void UpdateJoinInfo(bool info)
        {
            _gameStateInfo.JoiningSuccessful = info;
        }

        public void NextState()
        {
            CurrentGameState = CurrentGameState.GetNextState();
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