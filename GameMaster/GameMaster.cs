using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.ActionInfo;
using Common.Interfaces;
using GameMaster.ActionHandlers;
using GameMaster.Configuration;
using Messaging.InitialisationMessages;
using NLog;

namespace GameMaster
{
    public class GameMaster : IGameMaster
    {
        public VerboseLogger VerboseLogger { get; set; }

        private readonly GameConfiguration _gameConfiguration;

        private readonly MessagingHandler _messagingHandler;
        private Dictionary<Guid, int> _playerGuidToId;
        private List<(TeamColor team, PlayerType role)> _playersSlots;
        private IKnowledgeExchangeManager _knowledgeExchangeManager;
        private int _gameId;
        private bool _gameInProgress;
        private PieceGenerator _pieceGenerator;
        private Timer checkIfFullTeamTimer;
        private readonly string _gameName;

        public GameMaster(GameConfiguration gameConfiguration, ICommunicationClient communicationCommunicationClient, string gameName, LoggingMode loggingMode)
        {
            _gameConfiguration = gameConfiguration;
            _gameName = gameName;

            checkIfFullTeamTimer = new Timer(CheckIfGameFullCallback, null,
                (int) Constants.GameFullCheckStartDelay.TotalMilliseconds,
                (int) Constants.GameFullCheckInterval.TotalMilliseconds);

            _messagingHandler = new MessagingHandler(gameConfiguration, communicationCommunicationClient, HostNewGame);
            _messagingHandler.MessageReceived += (sender, args) => MessageHandler(args);

            VerboseLogger = new VerboseLogger(LogManager.GetCurrentClassLogger(), loggingMode);

            HostNewGame();
        }

        public GameMaster(GameMasterBoard board, Dictionary<Guid, int> playerGuidToId)
        {
            Board = board;

            _playerGuidToId = playerGuidToId;
        }

        public GameMasterBoard Board { get; private set; }

        public bool IsSlotAvailable()
        {
            return _playersSlots.Count > 0;
        }

        public (int gameId, Guid playerGuid, PlayerBase playerInfo) AssignPlayerToAvailableSlotWithPrefered(
            int playerId, TeamColor preferedTeam, PlayerType preferedRole)
        {
            (TeamColor team, PlayerType role) assignedValue;
            if (_playersSlots.Contains((preferedTeam, preferedRole)))
                assignedValue = (preferedTeam, preferedRole);
            else if (_playersSlots.Contains((preferedTeam, PlayerType.Member)))
                assignedValue = (preferedTeam, PlayerType.Member);
            else
                assignedValue = _playersSlots.First();

            _playersSlots.Remove(assignedValue);

            var playerInfo = new PlayerInfo(playerId, assignedValue.team, assignedValue.role);
            Board.Players.Add(playerId, playerInfo);

            var playerGuid = Guid.NewGuid();
            _playerGuidToId.Add(playerGuid, playerId);

            return (_gameId, playerGuid, playerInfo);
        }

        public void HandlePlayerDisconnection(int playerId)
        {
            VerboseLogger.Log($"Player {playerId} disconnected from game");

            if (!_playerGuidToId.ContainsValue(playerId))
                return;
            var disconnectedPlayerPair = _playerGuidToId.Single(x => x.Value == playerId);
            _playerGuidToId.Remove(disconnectedPlayerPair.Key);
        }

        public void RegisterGame()
        {
            _messagingHandler.CommunicationClient.Send(new RegisterGameMessage(new GameInfo(_gameName,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam,
                _gameConfiguration.GameDefinition.NumberOfPlayersPerTeam)));
        }

        public IKnowledgeExchangeManager KnowledgeExchangeManager { get; }
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
            _gameId = gameId;
        }

        public (BoardData data, bool isGameFinished) EvaluateAction(ActionInfo actionInfo)
        {
            var playerId = _playerGuidToId[actionInfo.PlayerGuid];
            var action = new ActionHandlerDispatcher((dynamic) actionInfo, Board, playerId, _knowledgeExchangeManager);
            return (data: action.Execute(), isGameFinished: Board.IsGameFinished());
        }

        public void MessageHandler(IMessage message)
        {
            // TODO Log all messages
            if (message is IRequest request)
                PutActionLog(request);

            IMessage response;
            lock (Board.Lock)
            {
                response = message.Process(this);

                if (_gameInProgress && Board.IsGameFinished())
                {
                    GameFinished?.Invoke(this, new GameFinishedEventArgs(Board.CheckWinner()));
                    FinishGame();
                }

                if (response != null)
                    _messagingHandler.CommunicationClient.Send(response);
            }
        }

        private void CheckIfGameFullCallback(object obj)
        {
            if (_playersSlots.Count > 0 || _gameInProgress) return;

            _gameInProgress = true;
            StartNewGame();

            var boardInfo = new BoardInfo(Board.Width, Board.TaskAreaSize, Board.GoalAreaSize);

            _messagingHandler.StartListeningToRequests(_playerGuidToId.Keys);
            foreach (var i in _playerGuidToId)
            {
                var playerLocation = Board.Players.Values.Single(x => x.Id == i.Value).Location;
                var gameStartMessage = new GameMessage(i.Value, Board.Players.Values, playerLocation, boardInfo);
                _messagingHandler.CommunicationClient.Send(gameStartMessage);
            }
        }

        private void StartNewGame()
        {
            var oldBoard = Board;
            var boardGenerator = new GameMasterBoardGenerator();
            Board = boardGenerator.InitializeBoard(_gameConfiguration.GameDefinition);
            foreach (var boardPlayer in oldBoard.Players)
            {
                var oldPlayerInfo = boardPlayer.Value;
                var playerInfo = new PlayerInfo(oldPlayerInfo.Id, oldPlayerInfo.Team, oldPlayerInfo.Role);
                Board.Players.Add(boardPlayer.Key, playerInfo);
            }

            boardGenerator.SpawnGameObjects(_gameConfiguration.GameDefinition);

            _pieceGenerator = new PieceGenerator(Board, _gameConfiguration.GameDefinition.ShamProbability,
                _gameConfiguration.GameDefinition.PlacingNewPiecesFrequency);
            _knowledgeExchangeManager = new KnowledgeExchangeManager();
        }

        private void HostNewGame()
        {
            FinishGame();
            var boardGenerator = new GameMasterBoardGenerator();
            Board = boardGenerator.InitializeBoard(_gameConfiguration.GameDefinition);
            _playersSlots =
                boardGenerator.GeneratePlayerSlots(_gameConfiguration.GameDefinition.NumberOfPlayersPerTeam);

            _playerGuidToId = new Dictionary<Guid, int>();
            foreach (var player in Board.Players) _playerGuidToId.Add(Guid.NewGuid(), player.Key);

            RegisterGame();
        }

        private void FinishGame()
        {
            if (_gameInProgress)
            {
                _gameInProgress = false;
                _pieceGenerator.SpawnTimer.Dispose();
            }
        }

        public virtual event EventHandler<GameFinishedEventArgs> GameFinished;

        public void PutActionLog(IRequest record)
        {
            var playerId = _playerGuidToId[record.PlayerGuid];
            var playerInfo = Board.Players[playerId];
            var actionLog = new RequestLog(record, playerId, playerInfo.Team, playerInfo.Role);
            VerboseLogger.Log(actionLog.ToLog());
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