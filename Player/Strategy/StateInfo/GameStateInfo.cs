using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Player.Interfaces;

namespace Player.Strategy.StateInfo
{
    public class GameStateInfo : BaseInfo
    {
        public string GameName { get; set; }
        public TeamColor Color { get; set;  }
        public IEnumerable<GameInfo> GameInfo { get; set; }
        public bool JoiningSuccessful { get; set; }
        public bool IsRunning { get; set; }
        public IStrategy PlayerStrategy { get; set; }
        private IPlayerStrategyFactory _playerStrategyFactory;

        public GameStateInfo(string gameName, IPlayerStrategyFactory playerStrategyFactory)
        {
            GameName = gameName;
            _playerStrategyFactory = playerStrategyFactory;
        }

        public void CreatePlayerStrategy()
        {
            PlayerStrategy = _playerStrategyFactory.CreatePlayerStrategy();
        }
    }
}
