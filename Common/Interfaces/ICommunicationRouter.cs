using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface ICommunicationRouter
    {
        IEnumerable<GameInfo> GetAllJoinableGames();
        int GetGameIdFor(string gameName);
        void RegisterNewGame(GameInfo gameInfo, int socketId);
        void UpdateTeamCount(int gameId, TeamColor team);
        void DeregisterGame(int gameId);
        void AssignGameIdToPlayerId(int gameId, int playerId);
        int GetGameIdFor(int playerId);
        IEnumerable<int> GetAllPlayersInGame(int gameId);
    }
}