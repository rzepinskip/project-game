using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    class CommunicationResolver : IResolver
    {
        private readonly Dictionary<int, int> _playerIdToGameId;
        private readonly Dictionary<int, GameInfo> _gameIdToGameInfo;

        public CommunicationResolver()
        {
            _playerIdToGameId = new Dictionary<int, int>();
            _gameIdToGameInfo = new Dictionary<int, GameInfo>();
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return _gameIdToGameInfo.Values;
        }

        public int GetGameId(string gameName)
        {
            Debug.WriteLine(gameName);
            return _gameIdToGameInfo.FirstOrDefault(x => x.Value.GameName == gameName).Key;
        }

        public void RegisterNewGame(GameInfo gameInfo, int id)
        {
            _gameIdToGameInfo.Add(id, gameInfo);
        }
        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _gameIdToGameInfo.TryGetValue(gameId, out var info);
            if (info == null)
                return;
            switch (team)
            {
            case TeamColor.Blue:
                info.BlueTeamPlayers--;
                break;
            case TeamColor.Red:
                info.RedTeamPlayers--;
                break;
            default:
                throw new Exception("Unexpected team color");
            }
            _gameIdToGameInfo[gameId] = info;
        }

        public void UnregisterGame(int gameId)
        {
            _gameIdToGameInfo.Remove(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _playerIdToGameId.Add(playerId, gameId);
        }

        public int GetGameIdForPlayer(int playerId)
        {
            _playerIdToGameId.TryGetValue(playerId, out var gameId);
            return gameId;
        }
    }
}
