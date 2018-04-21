using System.Collections.Generic;
using Common;
using Player.Interfaces;

namespace Player.Strategy.StateInfo
{
    public class GameStateInfo : BaseInfo
    {
        public string GameName { get; set; }
        public TeamColor Color { get; set; }
        public PlayerType Role { get; set; }
        public IEnumerable<GameInfo> GameInfo { get; set; }
        public bool JoiningSuccessful { get; set; }
        public bool IsRunning { get; set; }
        public IStrategy PlayerStrategy { get; set; }
        public IPlayerStrategyFactory PlayerStrategyFactory { get; set; }

        public GameStateInfo(string gameName, TeamColor color, PlayerType role)
        {
            GameName = gameName;
            Color = color;
            Role = role;
        }

        public void CreatePlayerStrategy()
        {
            PlayerStrategy = PlayerStrategyFactory.CreatePlayerStrategy();
        }
    }
}
