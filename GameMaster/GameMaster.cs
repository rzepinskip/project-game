using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitializationMessages;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        private readonly IErrorsMessagesFactory _errorsMessagesFactory;

        private readonly string _gameName;
        private readonly MessagingHandler _messagingHandler;
        private ActionHandlerDispatcher _actionHandler;

        private readonly GameHost _gameHost;
        private Dictionary<Guid, int> _playerGuidToId;
        private HashSet<Guid> _playersInformedAboutGameResult;

        public GameMaster(GameConfiguration gameConfiguration, ICommunicationClient communicationCommunicationClient,
            string gameName, IErrorsMessagesFactory errorsMessagesFactory, LoggingMode loggingMode)
        {
            _gameHost = new GameHost(gameName, gameConfiguration, StartGame);

            _errorsMessagesFactory = errorsMessagesFactory;

            _messagingHandler = new MessagingHandler(gameConfiguration, communicationCommunicationClient, HostNewGame);
            _messagingHandler.MessageReceived += (sender, args) => MessageHandler(args);

            VerboseLogger = new VerboseLogger(LogManager.GetCurrentClassLogger(), loggingMode);
            KnowledgeExchangeManager = new KnowledgeExchangeManager();
            HostNewGame();
        }

        /// <summary>
        ///     Only for tests
        /// </summary>
        public GameMaster(GameMasterBoard board, Dictionary<Guid, int> playerGuidToId)
        {
            Board = board;

            _playerGuidToId = playerGuidToId;
        }

        public VerboseLogger VerboseLogger { get; }

        public GameMasterBoard Board {
            get { return _gameHost.Board; }
            private set { _gameHost.Board = value; }
        }

        public void HandlePlayerDisconnection(int playerId)
        {
            VerboseLogger.Log($"Player {playerId} disconnected from game");

            if (!_playerGuidToId.ContainsValue(playerId))
                return;
            var disconnectedPlayerPair = _playerGuidToId.Single(x => x.Value == playerId);
            _playerGuidToId.Remove(disconnectedPlayerPair.Key);
        }

        public void StartGame()
        {
            _messagingHandler.StartListeningToRequests(_playerGuidToId.Keys);
            KnowledgeExchangeManager = new KnowledgeExchangeManager();
            _actionHandler = new ActionHandlerDispatcher(Board, KnowledgeExchangeManager);

            var boardInfo = new BoardInfo(Board.Width, Board.TaskAreaSize, Board.GoalAreaSize);
            foreach (var i in _playerGuidToId)
            {
                var playerLocation = Board.Players.Values.Single(x => x.Id == i.Value).Location;
                var gameStartMessage = new GameMessage(i.Value, Board.Players.Values, playerLocation, boardInfo);
                _messagingHandler.CommunicationClient.Send(gameStartMessage);
            }
        }

        public bool IsSlotAvailable()
        {
            return _gameHost.IsSlotAvailable();
        }

        public (int gameId, Guid playerGuid, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(
            int playerId, TeamColor preferredTeam, PlayerType preferredRole)
        {
            var playerGuid = Guid.NewGuid();
            _playerGuidToId.Add(playerGuid, playerId);
            var (gameId, playerInfo) = _gameHost.AssignPlayerToAvailableSlotWithPrefered(playerId, preferredTeam, preferredRole);

            return (gameId, playerGuid, playerInfo);
        }

        public void HostNewGame()
        {
            _playerGuidToId = new Dictionary<Guid, int>();
            _playersInformedAboutGameResult = new HashSet<Guid>();
            _gameHost.HostNewGame();
            RegisterGame();
        }

        public void RegisterGame()
        {
            _messagingHandler.CommunicationClient.Send(new RegisterGameMessage(_gameHost.CurrentGameInfo()));
        }

        public IKnowledgeExchangeManager KnowledgeExchangeManager { get; private set; }

        public int? Authorize(Guid playerGuid)
        {
            if (_playerGuidToId.ContainsKey(playerGuid))
                return _playerGuidToId[playerGuid];
            return null;
        }

        public void SendDataToInitiator(int initiatorId, IMessage message)
        {
            _messagingHandler.CommunicationClient.Send(message);
        }

        public bool PlayerIdExists(int playerId)
        {
            return _playerGuidToId.ContainsValue(playerId);
        }

        public void SetGameId(int gameId)
        {
            _gameHost.GameId = gameId;
        }

        public (BoardData data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            if (!_playerGuidToId.ContainsKey(actionInfo.PlayerGuid))
            {
                //Console.WriteLine("Message from old game arrived?");
                return (null, true);
            }

            var playerId = _playerGuidToId[actionInfo.PlayerGuid];
            var action = _actionHandler.Resolve((dynamic) actionInfo, playerId);
            var hasGameEnded = Board.IsGameFinished();
            if (hasGameEnded)
            {
                GameFinished?.Invoke(this, new GameFinishedEventArgs(Board.CheckWinner()));
                _gameHost.GameInProgress = false;

                _playersInformedAboutGameResult.Add(actionInfo.PlayerGuid);
                if (_playersInformedAboutGameResult.Count == _playerGuidToId.Count)
                {
                    HostNewGame();
                }
            }

            return (data: action.Respond(), isGameFinished: hasGameEnded);
        }

        public void MessageHandler(IMessage message)
        {
            // TODO Log all messages
            if (message is IRequestMessage request)
                PutActionLog(request);

            IMessage response;
            lock (Board.Lock)
            {
                response = message.Process(this);

                if (response != null)
                    _messagingHandler.CommunicationClient.Send(response);
            }
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutActionLog(IRequestMessage record)
        {
            var playerId = _playerGuidToId[record.PlayerGuid];
            var playerInfo = Board.Players[playerId];
            var actionLog = new RequestLog(record, playerId, playerInfo.Team, playerInfo.Role);
            VerboseLogger.Log(actionLog.ToLog());
        }

        public void LogGameResults(TeamColor winners)
        {
            foreach (var player in Board.Players.Values)
            {
                var result = GameResult.Defeat;
                var playerGuid = _playerGuidToId.FirstOrDefault(p => p.Value == player.Id).Key;

                if (player.Team == winners)
                    result = GameResult.Victory;

                VerboseLogger.Log(
                    $"{result}, {DateTime.Now}, {_gameHost.GameId}, {player.Id}, {playerGuid},{player.Team}, {player.Role}");
            }
        }
    }

    public class GameFinishedEventArgs : EventArgs
    {
        public GameFinishedEventArgs(TeamColor winners)
        {
            Winners = winners;
        }

        public TeamColor Winners { get; set; }
    }
}