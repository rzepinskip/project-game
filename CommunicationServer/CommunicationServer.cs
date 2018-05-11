using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IErrorsMessagesFactory _errorsMessagesFactory;
        private readonly IAsynchronousSocketListener _socketListener;

        public CommunicationServer(IMessageDeserializer messageDeserializer, TimeSpan keepAliveTimeout, int port,
            IErrorsMessagesFactory
                errorsMessagesFactory)
        {
            _errorsMessagesFactory = errorsMessagesFactory;
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

        public int GetGameIdFor(int connectionId)
        {
            return _communicationRouter.GetGameIdFor(connectionId);
        }

        public void DeregisterPlayerFromGame(int playerId)
        {
            _communicationRouter.DeregisterPlayerFromGame(playerId);
        }

        public void RegisterNewGame(GameInfo gameInfo, int connectionId)
        {
            _communicationRouter.RegisterNewGame(gameInfo, connectionId);
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

        public void Send(IMessage message, int connectionId)
        {
            try
            {
                _socketListener.Send(message, connectionId);
            }
            catch (Exception e)
            {
                if (e is SocketException socketException &&
                    socketException.SocketErrorCode == SocketError.ConnectionReset)
                {
                    var clientType = _communicationRouter.GetClientTypeFrom(connectionId);

                    Console.WriteLine($"{clientType} #{e.Data["connectionId"]} disconnected");

                    return;
                }

                throw;
            }
        }

        public void MarkClientAsPlayer(int connectionId)
        {
            _communicationRouter.MarkClientAsPlayer(connectionId);
        }

        public void MarkClientAsGameMaster(int connectionId)
        {
            _communicationRouter.MarkClientAsGameMaster(connectionId);
        }

        public void HandleMessage(IMessage message, int connectionId)
        {
            Logger.Info(message + " from  id: " + connectionId);
            message.Process(this, connectionId);
        }

        public void HandleConnectionError(Exception e)
        {
            var connectionId = (int) e.Data["connectionId"];
            var clientType = _communicationRouter.GetClientTypeFrom(connectionId);
            IMessage disconnectedMessage;

            if (clientType == ClientType.GameMaster)
            {
                var gameId = GetGameIdFor(connectionId);
                var clients = _communicationRouter.GetAllClientsConnectedWithGame(connectionId).ToList();
                clients.Remove(gameId);
                {
                    foreach (var playerId in clients)
                    {
                        disconnectedMessage = _errorsMessagesFactory.CreateGameMasterDisconnectedMessage(gameId);
                        Send(disconnectedMessage, playerId);
                        DeregisterPlayerFromGame(playerId);
                    }
                }
            }

            if (clientType == ClientType.Player)
            {
                disconnectedMessage = _errorsMessagesFactory.CreatePlayerDisconnectedMessage(connectionId);
                Send(disconnectedMessage, GetGameIdFor(connectionId));

                _socketListener.CloseSocket(connectionId);
            }
        }
    }
}