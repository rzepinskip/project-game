using System;
using System.Collections.Generic;
using System.Linq;
using Common;

namespace CommunicationServer
{
    internal class CommunicationRouter : ICommunicationRouter
    {
        private readonly Dictionary<int, GameInfo> _gameIdToGameInfo;
        private readonly Dictionary<int, ClientType> _connectionIdToClientType;
        private readonly Dictionary<int, int> _connectionIdToGameId;

        public CommunicationRouter()
        {
            _connectionIdToGameId = new Dictionary<int, int>();
            _gameIdToGameInfo = new Dictionary<int, GameInfo>();
            _connectionIdToClientType = new Dictionary<int, ClientType>();
        }

        public IEnumerable<GameInfo> GetAllJoinableGames()
        {
            return _gameIdToGameInfo.Values;
        }

        public int GetGameIdFor(string gameName)
        {
            return _gameIdToGameInfo.Single(x => x.Value.GameName == gameName).Key;
        }

        public void RegisterNewGame(GameInfo gameInfo, int connectionId)
        {
            var gameId = connectionId;
            _gameIdToGameInfo.Add(gameId, gameInfo);
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

        public void DeregisterGame(int gameId)
        {
            _gameIdToGameInfo.Remove(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _connectionIdToGameId.Add(playerId, gameId);
        }

        public int GetGameIdFor(int connectionId)
        {
            _connectionIdToGameId.TryGetValue(connectionId, out var gameId);
            return gameId;
        }

        public IEnumerable<int> GetAllClientsConnectedWithGame(int gameId)
        {
            return _connectionIdToGameId.Where(x => x.Value == gameId && x.Key != gameId).Select(x => x.Key);
        }

        public void MarkClientAsPlayer(int connectionId)
        {
            Console.WriteLine($"{connectionId} added as {ClientType.Player}");
            _connectionIdToClientType.TryAdd(connectionId, ClientType.Player);
        }

        public void MarkClientAsGameMaster(int connectionId)
        {
            Console.WriteLine($"{connectionId} added as {ClientType.GameMaster}");
            _connectionIdToClientType.TryAdd(connectionId, ClientType.GameMaster);
        }

        public ClientType GetClientTypeFrom(int connectionId)
        {
            return !_connectionIdToClientType.ContainsKey(connectionId)
                ? ClientType.NonInitialized
                : _connectionIdToClientType[connectionId];
        }
    }
}