using System.Collections.Generic;

namespace Common.Interfaces
{
    public interface IGamesManager
    {
        IEnumerable<GameInfo> GetAllJoinableGames();
        int GetGameIdFor(string gameName);
        void RegisterNewGame(GameInfo gameInfo, int connectionId);
        void UpdateTeamCount(int gameId, TeamColor team);
        void DeregisterGame(int gameId);
        void AssignGameIdToPlayerId(int gameId, int playerId);
        int GetGameIdFor(int playerId);
    }
}