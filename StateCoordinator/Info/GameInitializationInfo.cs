using System.Collections.Generic;
using Common;
using PlayerStateCoordinator.States;
using PlayerStateCoordinator.States.GameInitializationStates;

namespace PlayerStateCoordinator.Info
{
    public class GameInitializationInfo : BaseInfo
    {
        public readonly string GameName;

        public GameInitializationInfo(string gameName, TeamColor team, PlayerType role)
        {
            GameName = gameName;
            Team = team;
            Role = role;
        }

        public State PlayerGameInitializationBeginningState => new GetGamesState(this);

        public bool IsGameRunning { get; set; }
        public IEnumerable<GameInfo> GamesInfo { get; set; }
        public TeamColor Team { get; }
        public PlayerType Role { get; }
        public bool JoiningSuccessful { get; set; }
    }
}