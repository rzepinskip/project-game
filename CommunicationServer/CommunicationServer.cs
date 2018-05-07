using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private readonly Dictionary<int, ClientType> _clientTypes;
        private readonly ICommunicationRouter _communicationCommunicationRouter;
        private readonly IAsynchronousSocketListener _socketListener;

        public CommunicationServer(IMessageDeserializer messageDeserializer, TimeSpan keepAliveInterval, int port)
        {
            _clientTypes = new Dictionary<int, ClientType>();
            _socketListener = new AsynchronousSocketListener(port, keepAliveInterval,
                messageDeserializer, HandleMessage
            );
            _communicationCommunicationRouter = new CommunicationRouter();
            new Thread(() => _socketListener.StartListening(HandleConnectionError)).Start();
        }

        public IEnumerable<GameInfo> GetAllJoinableGames()
        {
            return _communicationCommunicationRouter.GetAllJoinableGames();
        }

        public int GetGameIdFor(string gameName)
        {
            return _communicationCommunicationRouter.GetGameIdFor(gameName);
        }

        public void RegisterNewGame(GameInfo gameInfo, int socketId)
        {
            _communicationCommunicationRouter.RegisterNewGame(gameInfo, socketId);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _communicationCommunicationRouter.UpdateTeamCount(gameId, team);
        }

        public void DeregisterGame(int gameId)
        {
            _communicationCommunicationRouter.DeregisterGame(gameId);
        }

        public void AssignGameIdToPlayerId(int gameId, int playerId)
        {
            _communicationCommunicationRouter.AssignGameIdToPlayerId(gameId, playerId);
        }

        public int GetGameIdFor(int socketId)
        {
            return _communicationCommunicationRouter.GetGameIdFor(socketId);
        }

        public IEnumerable<int> GetAllPlayersInGame(int gameId)
        {
            return _communicationCommunicationRouter.GetAllPlayersInGame(gameId);
        }

        public void MarkClientAsPlayer(int socketId)
        {
            Console.WriteLine($"{socketId} added as {ClientType.Player}");
            _clientTypes.TryAdd(socketId, ClientType.Player);
        }

        public void MarkClientAsGameMaster(int socketId)
        {
            Console.WriteLine($"{socketId} added as {ClientType.GameMaster}");
            _clientTypes.TryAdd(socketId, ClientType.GameMaster);
        }

        public ClientType GetClientTypeFrom(int socketId)
        {
            return !_clientTypes.ContainsKey(socketId) ? ClientType.NonInitialized : _clientTypes[socketId];
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
                    socketException.SocketErrorCode == SocketError.ConnectionReset || e is ObjectDisposedException)
                {
                    var clientType = GetClientTypeFrom(socketId);

                    Console.WriteLine($"{clientType} #{e.Data["socketId"]} disconnected");

                    return;
                }

                throw;
            }
        }

        public void HandleMessage(IMessage message, int socketId)
        {
            Logger.Info(message + " from  id: " + socketId);
            message.Process(this, socketId);
        }

        public void HandleConnectionError(Exception e)
        {
            var socketId = (int)e.Data["socketId"];
            var clientType = GetClientTypeFrom(socketId);

            if (clientType == ClientType.GameMaster)
            {
                var playersInGame = GetAllPlayersInGame(socketId);
                foreach (var i in playersInGame)
                {
                    _socketListener.CloseSocket(i);
                }
            }

            if (clientType == ClientType.Player)
            {
                _socketListener.CloseSocket(socketId);
            }

        }
    }
}