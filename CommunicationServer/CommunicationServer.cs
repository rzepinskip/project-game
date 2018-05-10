using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using NLog;

namespace CommunicationServer
{
    public class CommunicationServer : ICommunicationServer
    {
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();

        private readonly ICommunicationRouter _communicationRouter;
        private readonly IAsynchronousSocketListener _socketListener;

        public CommunicationServer(IMessageDeserializer messageDeserializer, TimeSpan keepAliveTimeout, int port)
        {
            _socketListener = new AsynchronousSocketListener(port, keepAliveTimeout,
                messageDeserializer, HandleMessage
            );
            _communicationRouter = new CommunicationRouter();
            new Thread(() => _socketListener.StartListening(HandleConnectionError)).Start();
        }

        public IEnumerable<GameInfo> GetAllJoinableGames()
        {
            return _communicationRouter.GetAllJoinableGames();
        }

        public int GetGameIdFor(string gameName)
        {
            return _communicationRouter.GetGameIdFor(gameName);
        }

        public void RegisterNewGame(GameInfo gameInfo, int socketId)
        {
            _communicationRouter.RegisterNewGame(gameInfo, socketId);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _communicationRouter.UpdateTeamCount(gameId, team);
        }

        public void DeregisterGame(int gameId)
        {
            _communicationRouter.DeregisterGame(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _communicationRouter.AssignGameIdToPlayerId(gameId, playerId);
        }

        public int GetGameIdFor(int socketId)
        {
            return _communicationRouter.GetGameIdFor(socketId);
        }

        public void Send(IMessage message, int socketId)
        {
            try
            {
                _socketListener.Send(message, socketId);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    var clientType = _communicationRouter.GetClientTypeFrom(socketId);

                    Console.WriteLine($"{clientType} #{e.Data["socketId"]} disconnected");

                    return;
                }

                throw;
            }
        }

        public void MarkClientAsPlayer(int socketId)
        {
            _communicationRouter.MarkClientAsPlayer(socketId);
        }

        public void MarkClientAsGameMaster(int socketId)
        {
            _communicationRouter.MarkClientAsGameMaster(socketId);
        }

        public IEnumerable<int> GetAllPlayersInGame(int gameId)
        {
            return _communicationRouter.GetAllPlayersInGame(gameId);
        }

        public void HandleMessage(IMessage message, int socketId)
        {
            Logger.Info(message + " from  id: " + socketId);
            message.Process(this, socketId);
        }

        public void HandleConnectionError(Exception e)
        {
            var socketId = (int) e.Data["socketId"];
            var clientType = _communicationRouter.GetClientTypeFrom(socketId);

            if (clientType == ClientType.GameMaster)
            {
                var playersInGame = GetAllPlayersInGame(socketId);
                foreach (var i in playersInGame) _socketListener.CloseSocket(i);
            }

            if (clientType == ClientType.Player) _socketListener.CloseSocket(socketId);
        }
    }
}