using System;
using System.Collections.Generic;
using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IPlayer
    {
        IPlayerBoard Board { get; }
        void UpdateGameState(IEnumerable<GameInfo> gameInfo);
        void ChangePlayerCoordinatorState();
        void UpdateJoiningInfo(bool info);
        void NotifyAboutGameEnd();
        void UpdatePlayer(int playerid, Guid playerGuid, PlayerBase playerBase, int gameId);
        void UpdatePlayerGame(PlayerBase[] players, Location playerLocation, BoardInfo board);
    }
}