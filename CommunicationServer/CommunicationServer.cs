using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using CommunicationServer.Accepters;
using NLog;

namespace CommunicationServer
{
    public class CommunicationServer : ICommunicationServer, IConnectionTimeoutable
    {
        public static readonly ILogger Logger = LogManager.GetCurrentClassLogger();
        private readonly IResolver _communicationResolver;
        private readonly IAsynchronousSocketListener _listener;

        private Dictionary<int, ClientType> clientTypes;

        public CommunicationServer(IMessageDeserializer messageDeserializer, double keepAliveInterval, int port)
        {
            clientTypes = new Dictionary<int, ClientType>();
            _listener = new AsynchronousSocketListener(new TcpSocketAccepter(HandleMessage, messageDeserializer,
                TimeSpan.FromMilliseconds(keepAliveInterval), this, port));
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

        public void MarkClientAsPlayer(int id)
        {
            Console.WriteLine($"{id} added as {ClientType.Player}");
            clientTypes.TryAdd(id, ClientType.Player);
        }

        public void MarkClientAsGameMaster(int id)
        {
            Console.WriteLine($"{id} added as {ClientType.GameMaster}");
            clientTypes.TryAdd(id, ClientType.GameMaster);
        }

        public ClientType GetClientTypeFrom(int id)
        {
            return !clientTypes.ContainsKey(id) ? ClientType.NonInitialized : clientTypes[id];
        }

        public void Send(IMessage message, int id)
        {
            try
            {
                _listener.Send(message, id);

            }
            catch (Exception e)
            {
                if (e is SocketException socketException && socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    var clientType = GetClientTypeFrom(id);

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
            Console.WriteLine($"Socket #{socketId} exceeded keep alive timeout");
            //throw new ConnectionException("");
        }

        public void HandleMessage(IMessage message, int i)
        {
            Debug.WriteLine("CS Message received from: " + i);
            Logger.Info(message + " from  id: " + i);
            message.Process(this, i);
        }
    }
}