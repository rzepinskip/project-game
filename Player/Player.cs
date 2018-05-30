using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Player.Logging;
using Player.StrategyGroups;
using PlayerStateCoordinator;

namespace Player
{
    public class Player : PlayerBase, IPlayer
    {
        private readonly IErrorsMessagesFactory _errorsMessagesFactory;
        private readonly string _gameName;
        private readonly TeamColor _preferredColor;
        private readonly PlayerType _preferredRole;
        private readonly StrategyGroup _strategyGroup;
        private bool _gameFinished;
        private bool _gameStarted;
        private StateCoordinator _stateCoordinator;

        public Player(ICommunicationClient communicationClient, string gameName, TeamColor preferredColor,
            PlayerType preferredRole,
            IErrorsMessagesFactory errorsMessagesFactory, LoggingMode loggingMode, StrategyGroup strategyGroup)
        {
            CommunicationClient = communicationClient;
            _gameName = gameName;
            _preferredColor = preferredColor;
            _preferredRole = preferredRole;
            _errorsMessagesFactory = errorsMessagesFactory;
            _strategyGroup = strategyGroup;

            var factory = new LoggerFactory();
            VerboseLogger = new VerboseLogger(factory.GetPlayerLogger(0), loggingMode);

            _stateCoordinator = new StateCoordinator(gameName, preferredColor, preferredRole);
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
            CommunicationClient.Send(_stateCoordinator.Start());
        }

        /// <summary>
        ///     Only for tests
        /// </summary>
        public Player(int id, Guid guid, TeamColor team, PlayerType preferredRole,
            PlayerBoard board, Location location)
        {
            Id = id;
            Team = team;
            Role = preferredRole;
            PlayerGuid = guid;
            GameId = 0;
            PlayerBoard = board;
            PlayerBoard.Players[id] = new PlayerInfo(id, team, preferredRole, location);

            _stateCoordinator = new StateCoordinator("", team, preferredRole);
        }

        public VerboseLogger VerboseLogger { get; private set; }

        public int GameId { get; private set; }
        public PlayerBoard PlayerBoard { get; private set; }
        public Guid PlayerGuid { get; private set; }

        public ICommunicationClient CommunicationClient { get; }
        public IPlayerBoard Board => PlayerBoard;

        public void UpdateGameState(IEnumerable<GameInfo> gameInfo)
        {
            _stateCoordinator.UpdateGamesInfo(gameInfo);
        }

        public void UpdateJoiningInfo(bool info)
        {
            _stateCoordinator.UpdateJoiningResult(info);
        }

        public void NotifyAboutGameEnd()
        {
            if (_gameStarted)
            {
                _stateCoordinator.NotifyAboutGameEnd();
                RestartStateCoordinator();
                CommunicationClient.Send(_stateCoordinator.Start());
                _gameStarted = false;
            }

            _gameFinished = true;
        }

        public void UpdatePlayer(int playerId, Guid playerGuid, PlayerBase playerBase, int gameId)
        {
            Id = playerId;
            PlayerGuid = playerGuid;
            Team = playerBase.Team;
            Role = playerBase.Role;
            GameId = gameId;

            Console.Title = $"Player #{Id} [{Team}][{Role}]";
        }

        public void InitializeGameData(Location playerLocation, BoardInfo board, IEnumerable<PlayerBase> players)
        {
            PlayerBoard = new PlayerBoard(board.Width, board.TasksHeight, board.GoalsHeight);
            var playerBases = players.ToList();
            foreach (var playerBase in playerBases) PlayerBoard.Players.Add(playerBase.Id, new PlayerInfo(playerBase));

            PlayerBoard.Players[Id].Location = playerLocation;

            var playerStrategy = _strategyGroup.GetStrategyFor(this, PlayerBoard, PlayerGuid, GameId, playerBases);
            Console.WriteLine("Player has chosen " + playerStrategy.GetType().Name);

            _stateCoordinator.UpdatePlayerStrategyBeginningState(playerStrategy.GetBeginningState());
            _stateCoordinator.CurrentState = playerStrategy.GetBeginningState();

            _gameStarted = true;
            Console.WriteLine("Player has updated game data and started playing");
        }

        public void HandleGameMasterDisconnection()
        {
            VerboseLogger.Log($"GM for game {GameId} disconnected");
            RestartStateCoordinator();
        }

        public void InitializePlayer(int id, Guid guid, TeamColor team, PlayerType role, PlayerBoard board,
            Location location, LoggingMode loggingMode)
        {
            var factory = new LoggerFactory();
            VerboseLogger = new VerboseLogger(factory.GetPlayerLogger(id), loggingMode);

            Id = id;
            Team = team;
            Role = role;
            PlayerGuid = guid;
            GameId = 0;
            PlayerBoard = board;
            PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);
            _stateCoordinator = new StateCoordinator("", team, role);
        }

        private void HandleResponse(IMessage message)
        {
            message.Process(this);

            var responsesToSend = _stateCoordinator.Process(message).ToList();

            if (_gameFinished)
            {
                _gameFinished = false;
                return;
            }

            foreach (var response in responsesToSend)
            {
                VerboseLogger.Log(response.ToLog());
                CommunicationClient.Send(response);
            }
        }

        public void HandleConnectionError(CommunicationException e)
        {
            CommunicationClient.HandleCommunicationError(e);

            if (e.Severity == CommunicationException.ErrorSeverity.Temporary)
                return;
            RestartStateCoordinator();
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
            CommunicationClient.Send(_stateCoordinator.Start());
        }

        private void RestartStateCoordinator()
        {
            _stateCoordinator.StopTimers();
            _stateCoordinator = new StateCoordinator(_gameName, _preferredColor, _preferredRole);
        }
    }
}