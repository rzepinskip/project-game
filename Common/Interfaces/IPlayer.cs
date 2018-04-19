using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface IPlayer
    {
        IPlayerBoard Board { get; }
        void UpdateGameState(IEnumerable<GameInfo> gameInfo);
        void ChangePlayerCoordinatorState();
        void UpdateJoiningInfo(bool info);
    }
}