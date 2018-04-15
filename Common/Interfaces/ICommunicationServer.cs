using System;
using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICommunicationServer
    {
        void Send(IMessage message, int id);
        void SetupServer(Action<IMessage, int> messageHandler);
        IEnumerable<GameInfo> GetGames();
        int GetGameId(string gameName);
        void RegisterNewGame(GameInfo gameInfo, int id);
        void UpdateTeamCount(int gameId, TeamColor team);
        void UnregisterGame(int gameId);
        void AssignGameIdToPlayerId(int gameId, int playerId);
        int GetGameIdForPlayer(int playerId);
        void StartListening();
    }
}
