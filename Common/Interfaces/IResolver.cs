using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface IResolver
    {
        IEnumerable<GameInfo> GetGames();
        int GetGameId(string gameName);
        void RegisterNewGame(GameInfo gameInfo, int socketId);
        void UpdateTeamCount(int gameId, TeamColor team);
        void UnregisterGame(int gameId);
        void AssignGameIdToPlayerId(int gameId, int playerId);
        int GetGameIdFor(int playerId);
    }
}