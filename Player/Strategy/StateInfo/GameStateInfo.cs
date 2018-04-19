using System;
using System.Collections.Generic;
using System.Text;
using Common;

namespace Player.Strategy.StateInfo
{
    public class GameStateInfo : BaseInfo
    {
        public string GameName { get; set; }
        public TeamColor Color { get; set;  }
        public IEnumerable<GameInfo> GameInfo { get; set; }
        public bool JoiningSuccessful { get; set; }

        public GameStateInfo(string gameName)
        {
            GameName = gameName;
        }
    }
}
