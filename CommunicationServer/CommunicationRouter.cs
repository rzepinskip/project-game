using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Common;

namespace CommunicationServer
{
    internal class CommunicationRouter : ICommunicationRouter
    {
        private readonly Dictionary<int, ClientType> _connectionIdToClientType;

        private readonly Dictionary<int, int> _connectionIdToGameId;

        private readonly Dictionary<int, GameInfo> _gameIdToGameInfo;

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

        public int GetGameIdFor(int connectionId)
        {
            return _connectionIdToGameId[connectionId];
        }

        public bool RegisterNewGame(GameInfo gameInfo, int connectionId)
        {
            if (_gameIdToGameInfo.Values.Count(x => x.GameName == gameInfo.GameName) > 0)
                return false;
            var gameId = connectionId;
            _gameIdToGameInfo.Add(gameId, gameInfo);
            if(!_connectionIdToGameId.ContainsKey(connectionId))
                _connectionIdToGameId.Add(connectionId, gameId);
            return true;
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
            if(!_connectionIdToGameId.ContainsKey(playerId))
                _connectionIdToGameId.Add(playerId, gameId);
        }

        public IEnumerable<int> GetAllClientsConnectedWithGame(int gameId)
        {
            return _connectionIdToGameId.Where(x => x.Value == gameId && x.Key != gameId).Select(x => x.Key);
        }

        public void MarkClientAsPlayer(int connectionId)
        {
            if (_connectionIdToClientType.ContainsKey(connectionId)) return;

            Console.WriteLine($"{connectionId} added as {ClientType.Player}");
            _connectionIdToClientType.Add(connectionId, ClientType.Player);
        }

        public void MarkClientAsGameMaster(int connectionId)
        {
            if (_connectionIdToClientType.ContainsKey(connectionId)) return;

            Console.WriteLine($"{connectionId} added as {ClientType.GameMaster}");
            _connectionIdToClientType.Add(connectionId, ClientType.GameMaster);
        }

        public ClientType GetClientTypeFrom(int connectionId)
        {
            return !_connectionIdToClientType.ContainsKey(connectionId)
                ? ClientType.NonInitialized
                : _connectionIdToClientType[connectionId];
        }

        public void DeregisterPlayerFromGame(int playerId)
        {
            _connectionIdToGameId.Remove(playerId);
        }
    }
}