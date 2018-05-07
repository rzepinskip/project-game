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
    public class CommunicationServer : ICommunicationServer, IConnectionTimeoutable
    {
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly ICommunicationRouter _communicationCommunicationRouter;
        private readonly IAsynchronousSocketListener _listener;

        private readonly Dictionary<int, ClientType> _clientTypes;

        public CommunicationServer(IMessageDeserializer messageDeserializer, double keepAliveInterval, int port)
        {
            _clientTypes = new Dictionary<int, ClientType>();
            _listener = new AsynchronousSocketListener(HandleMessage, messageDeserializer,
                TimeSpan.FromMilliseconds(keepAliveInterval), this, port);
            _communicationCommunicationRouter = new CommunicationRouter();
            new Thread(() => _listener.StartListening()).Start();
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
                _listener.Send(message, socketId);

            }
            catch (Exception e)
            {
                if (e is SocketException socketException && socketException.SocketErrorCode == SocketError.ConnectionReset || e is ObjectDisposedException)
                {
                    var clientType = GetClientTypeFrom(socketId);

                    Console.WriteLine($"{clientType} #{e.Data["socketId"]} disconnected");

                    return;
                }

                throw;
            }
        }

        public void StartListening()
        {
            _listener.StartListening();
        }

        public void HandleConnectionTimeout(int socketId)
        {
            Console.WriteLine($"{GetClientTypeFrom(socketId)} #{socketId} exceeded keep alive timeout");
        }

        public void HandleMessage(IMessage message, int socketId)
        {
            Debug.WriteLine("CS Message received from: " + socketId);
            Logger.Info(message + " from  id: " + socketId);
            message.Process(this, socketId);
        }
    }
}