using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using Common;
using Common.Interfaces;
using Communication;
using Communication.Errors;
using NLog;

namespace CommunicationServer
{
    public class CommunicationServer : ICommunicationServer
    {
        public static VerboseLogger VerboseLogger;
        private readonly ICommunicationRouter _communicationRouter;

        private readonly IErrorsMessagesFactory _errorsMessagesFactory;
        private readonly IAsynchronousSocketListener _socketListener;

        public CommunicationServer(IMessageDeserializer messageDeserializer, TimeSpan keepAliveTimeout, int port,
            IErrorsMessagesFactory
                errorsMessagesFactory, LoggingMode loggingMode, IPAddress address)
        {
            VerboseLogger = new VerboseLogger(LogManager.GetCurrentClassLogger(), loggingMode);

            _errorsMessagesFactory = errorsMessagesFactory;
            _socketListener = new AsynchronousSocketListener(port, keepAliveTimeout,
                messageDeserializer, HandleMessage, address
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

        public bool RegisterNewGame(GameInfo gameInfo, int connectionId)
        {
            return _communicationRouter.RegisterNewGame(gameInfo, connectionId);
        }

        public void UpdateTeamCount(int gameId, TeamColor team)
        {
            _communicationRouter.UpdateTeamCount(gameId, team);
        }

        public void DeregisterGame(int connectionId)
        {
            _communicationRouter.DeregisterGame(connectionId);
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
                if (e is CommunicationException ce)
                {
                    HandleConnectionError(ce);
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
            VerboseLogger.Log(message + " from  id: " + connectionId);
            message.Process(this, connectionId);
        }

        public void HandleConnectionError(CommunicationException e)
        {
            if (!(e is IdentifiableCommunicationException))
                throw e;

            var ice = (IdentifiableCommunicationException) e;
            var connectionId = ice.ConnectionId;

            if (ice.Severity == CommunicationException.ErrorSeverity.Temporary)
            {
                VerboseLogger.Log($"Encountered temporary problem with connection #{connectionId}: {ice.Message}");
                return;
            }

            if (!_socketListener.IsConnectionExistent(connectionId))
            {
                VerboseLogger.Log("Non existent connenctionId in HandleConnectionError");
                return;
            }

            VerboseLogger.Log($"Handling disconnection event for connection #{connectionId}: {e.Message}");

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
                DeregisterPlayerFromGame(connectionId);
            }

            _socketListener.CloseConnection(connectionId);
        }
    }
}