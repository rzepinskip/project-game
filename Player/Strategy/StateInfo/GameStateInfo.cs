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
        public  IPlayerStrategyFactory PlayerStrategyFactory { get; set; }

        public GameStateInfo(string gameName, TeamColor color)
        {
            GameName = gameName;
            Color = color;
        }

        public void CreatePlayerStrategy()
        {
            PlayerStrategy = PlayerStrategyFactory.CreatePlayerStrategy();
        }
    }
}
