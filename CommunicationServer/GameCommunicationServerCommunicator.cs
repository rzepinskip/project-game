using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using CommunicationServer.Accepters;
using NLog;

namespace CommunicationServer
{
    public class GameCommunicationServerCommunicator : ICommunicationServerCommunicator
    {
        private static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IServerCommunicator _listener;
        private readonly IResolver _communicationResolver;

        public GameCommunicationServerCommunicator(IMessageDeserializer messageDeserializer)
        {
            _listener = new AsynchronousSocketListener(new TcpSocketAccepter(HandleMessage, messageDeserializer), 1000000000);
            _communicationResolver = new CommunicationResolver();
            new Thread(() => _listener.StartListening()).Start();
        }

        public IEnumerable<GameInfo> GetGames()
        {
            return _communicationResolver.GetGames();
        }

        public int GetGameId(string gameName)
        {
            return _communicationResolver.GetGameId(gameName);
        }

        public void RegisterNewGame(GameInfo gameInfo, int id)
        {
            _communicationResolver.RegisterNewGame(gameInfo, id);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _communicationResolver.UpdateTeamCount(gameId, team);
        }

        public void UnregisterGame(int gameId)
        {
            _communicationResolver.UnregisterGame(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _communicationResolver.AssignGameIdToPlayerId(gameId, playerId);
        }

        public int GetGameIdForPlayer(int playerId)
        {
            return _communicationResolver.GetGameIdForPlayer(playerId);
        }

        public void Send(IMessage message, int id)
        {
            _listener.Send(message, id);
        }

        public void StartListening()
        {
            _listener.StartListening();
        }

        public void HandleMessage(IMessage message, int i)
        {
            Debug.WriteLine("CS Message received from: " + i);
            Logger.Info(message + " from  id: " + i);
            message.Process(this, i);
        }

        public void HandleCallback(IAsyncResult ar)
        {
        }
    }
}