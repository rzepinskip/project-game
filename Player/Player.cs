using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Player.Logging;
using PlayerStateCoordinator;

namespace Player
{
    public class Player : PlayerBase, IPlayer
    {
        private readonly TeamColor _color;
        private readonly IErrorsMessagesFactory _errorsMessagesFactory;
        private readonly string _gameName;
        private readonly PlayerType _role;
        private StateCoordinator _stateCoordinator;

        public Player(ICommunicationClient communicationClient, string gameName, TeamColor color, PlayerType role,
            IErrorsMessagesFactory errorsMessagesFactory, LoggingMode loggingMode)
        {
            CommunicationClient = communicationClient;
            _gameName = gameName;
            _color = color;
            _role = role;
            _errorsMessagesFactory = errorsMessagesFactory;

            var factory = new LoggerFactory();
            VerboseLogger = new VerboseLogger(factory.GetPlayerLogger(0), loggingMode);

            _stateCoordinator = new StateCoordinator(gameName, color, role);
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
            CommunicationClient.Send(_stateCoordinator.Start());
        }

        /// <summary>
        ///     Only for tests
        /// </summary>
        public Player(int id, Guid guid, TeamColor team, PlayerType role,
            PlayerBoard board, Location location)
        {
            Id = id;
            Team = team;
            Role = role;
            PlayerGuid = guid;
            GameId = 0;
            PlayerBoard = board;
            PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);

            _stateCoordinator = new StateCoordinator("", team, role);
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
            Console.WriteLine("Game finished");
            _stateCoordinator = new StateCoordinator(_gameName, _color, _role);
        }

        public void UpdatePlayer(int playerId, Guid playerGuid, PlayerBase playerBase, int gameId)
        {
            Id = playerId;
            PlayerGuid = playerGuid;
            Team = playerBase.Team;
            Role = playerBase.Role;
            GameId = gameId;

            Console.Title = $"Player #{Id} [{Team}]";
        }

        public void InitializeGameData(Location playerLocation, BoardInfo board, IEnumerable<PlayerBase> players)
        {
            PlayerBoard = new PlayerBoard(board.Width, board.TasksHeight, board.GoalsHeight);
            foreach (var playerBase in players) PlayerBoard.Players.Add(playerBase.Id, new PlayerInfo(playerBase));

            PlayerBoard.Players[Id].Location = playerLocation;

            var playerStrategy = new PlayerStrategy(this, PlayerBoard, PlayerGuid, GameId);
            _stateCoordinator.UpdatePlayerStrategyBeginningState(playerStrategy.GetBeginningState());
            _stateCoordinator.CurrentState = playerStrategy.GetBeginningState();

            Console.WriteLine("Player has updated game data and started playing");
        }

        public void HandleGameMasterDisconnection()
        {
            VerboseLogger.Log($"GM for game {GameId} disconnected");
            _stateCoordinator = new StateCoordinator(_gameName, _color, _role);
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

            _stateCoordinator = new StateCoordinator(_gameName, _color, _role);
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
            CommunicationClient.Send(_stateCoordinator.Start());
        }
    }
}