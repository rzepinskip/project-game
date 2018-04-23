using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common;
using Common.Interfaces;
using NLog;

namespace CommunicationServer
{

    public class GameCommunicationServer : ICommunicationServer
    {
        private readonly IServer _listener;
        private readonly IResolver CommunicationResolver;
        private static ILogger _logger = LogManager.GetCurrentClassLogger();
        public GameCommunicationServer()
        {
            _listener = new AsynchronousSocketListener(new CommunicationServerConverter(), HandleMessage, 1000000000);
            CommunicationResolver = new CommunicationResolver();
            new Thread(() => _listener.StartListening()).Start();
        }

        public void HandleMessage(IMessage message, int i)
        {
            Debug.WriteLine("CS Message received from: " + i);
            _logger.Info(message.ToString() + " from  id: " + i);
            message.Process(this, i);
        }

        public void HandleCallback(IAsyncResult ar)
        {

        }

        public IEnumerable<GameInfo> GetGames()
        {
            return CommunicationResolver.GetGames();
        }

        public int GetGameId(string gameName)
        {
            return CommunicationResolver.GetGameId(gameName);
        }

        public void RegisterNewGame(GameInfo gameInfo, int id)
        {
            CommunicationResolver.RegisterNewGame(gameInfo, id);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            CommunicationResolver.UpdateTeamCount(gameId, team);
        }

        public void UnregisterGame(int gameId)
        {
            CommunicationResolver.UnregisterGame(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            CommunicationResolver.AssignGameIdToPlayerId(gameId, playerId);
        }

        public int GetGameIdForPlayer(int playerId)
        {
            return CommunicationResolver.GetGameIdForPlayer(playerId);
        }

        public void Send(IMessage message, int id)
        {
            _listener.Send(message, id);
        }


        public void StartListening()
        {
            _listener.StartListening();
        }
    }
}
