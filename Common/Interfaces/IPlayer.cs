using System;
using System.Collections.Generic;
using Common.BoardObjects;

namespace Common.Interfaces
{
    public interface IPlayer
    {
        IPlayerBoard Board { get; }
        void UpdateGameState(IEnumerable<GameInfo> gameInfo);
        void UpdateJoiningInfo(bool info);
        void NotifyAboutGameEnd();
        void UpdatePlayer(int playerid, Guid playerGuid, PlayerBase playerBase, int gameId);
        void InitializeGameData(Location playerLocation, BoardInfo board, IEnumerable<PlayerBase> players);
        void HandleKnowledgeExchangeRequest(int initiatorId);
        void HandleGameMasterDisconnection();
    }
}