﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Common;
using Common.Interfaces;

namespace CommunicationServer
{
    internal class CommunicationRouter : ICommunicationRouter
    {
        private readonly Dictionary<int, GameInfo> _gameIdToGameInfo;
        private readonly Dictionary<int, int> _socketIdToGameId;

        public CommunicationRouter()
        {
            _socketIdToGameId = new Dictionary<int, int>();
            _gameIdToGameInfo = new Dictionary<int, GameInfo>();
        }

        public IEnumerable<GameInfo> GetAllJoinableGames()
        {
            return _gameIdToGameInfo.Values;
        }

        public int GetGameIdFor(string gameName)
        {
            return _gameIdToGameInfo.Single(x => x.Value.GameName == gameName).Key;
        }

        public void RegisterNewGame(GameInfo gameInfo, int socketId)
        {
            var gameId = socketId;
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
            _socketIdToGameId.Add(playerId, gameId);
        }

        public int GetGameIdFor(int socketId)
        {
            _socketIdToGameId.TryGetValue(socketId, out var gameId);
            return gameId;
        }

        public IEnumerable<int> GetAllPlayersInGame(int gameId)
        {
            return _socketIdToGameId.Where(x => x.Value == gameId && x.Key != gameId).Select(x => x.Key);
        }
    }
}