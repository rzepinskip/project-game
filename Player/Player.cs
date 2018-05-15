using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Messaging;
using Messaging.KnowledgeExchangeMessages;
using Player.Logging;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase, IPlayer
    {
        private bool _hasGameEnded;
        private PlayerCoordinator _playerCoordinator;
        public VerboseLogger VerboseLogger;
        private readonly string _gameName;
        private readonly TeamColor _color;
        private readonly PlayerType _role;
        public Player(ICommunicationClient communicationClient, string gameName, TeamColor color, PlayerType role, LoggingMode loggingMode)
        {
            CommunicationClient = communicationClient;
            _gameName = gameName;
            _color = color;
            _role = role;

            var factory = new LoggerFactory();
            VerboseLogger =   new VerboseLogger(factory.GetPlayerLogger(0), loggingMode);

            _playerCoordinator = new PlayerCoordinator(gameName, color, role);
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
        }

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

            _playerCoordinator = new PlayerCoordinator("", team, role);
        }

        public int GameId { get; private set; }
        public PlayerBoard PlayerBoard { get; private set; }
        public Guid PlayerGuid { get; private set; }

        public ICommunicationClient CommunicationClient { get; }
        public IPlayerBoard Board => PlayerBoard;

        public void UpdateGameState(IEnumerable<GameInfo> gameInfo)
        {
            _playerCoordinator.UpdateGameStateInfo(gameInfo);
        }

        public void UpdateJoiningInfo(bool info)
        {
            _playerCoordinator.UpdateJoinInfo(info);
        }

        public void NotifyAboutGameEnd()
        {
            _hasGameEnded = true;
            _playerCoordinator.NotifyAboutGameEnd();
        }

        public void UpdatePlayer(int playerId, Guid playerGuid, PlayerBase playerBase, int gameId)
        {
            Id = playerId;
            PlayerGuid = playerGuid;
            Team = playerBase.Team;
            Role = playerBase.Role;
            GameId = gameId;
        }

        public void InitializeGameData(Location playerLocation, BoardInfo board, IEnumerable<PlayerBase> players)
        {
            PlayerBoard = new PlayerBoard(board.Width, board.TasksHeight, board.GoalsHeight);
            foreach (var playerBase in players) PlayerBoard.Players.Add(playerBase.Id, new PlayerInfo(playerBase));

            PlayerBoard.Players[Id].Location = playerLocation;
            _playerCoordinator.CreatePlayerStrategyFactory(new PlayerStrategyFactory(this));

            Debug.WriteLine("Player has updated game data and started playing");
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

            _playerCoordinator = new PlayerCoordinator("", team, role);
        }

        public IMessage GetNextRequestMessage()
        {
            return _playerCoordinator.NextMove();
        }

        public void HandleKnowledgeExchangeRequest(int initiatorId)
        {
            IMessage knowledgeExchangeResponse = null;
            if (Role == PlayerType.Leader)
                knowledgeExchangeResponse = DataMessage.FromBoardData(PlayerBoard.ToBoardData(Id, initiatorId), false, PlayerGuid);
            else
                knowledgeExchangeResponse = new RejectKnowledgeExchangeMessage(Id, initiatorId);
            CommunicationClient.Send(knowledgeExchangeResponse);
        }

        public void HandleGameMasterDisconnection()
        {
            VerboseLogger.Log($"GM for game {GameId} disconnected");
            _playerCoordinator = new PlayerCoordinator(_gameName, _color, _role);
        }

        private void HandleResponse(IMessage response)
        {
            //if (_hasGameEnded)
            //{
            //    _hasGameEnded = true;
            //    return;
            //}

            response.Process(this);

            if (_playerCoordinator.StrategyReturnsMessage())
            {
                var request = GetNextRequestMessage();
                VerboseLogger.Log(request.ToLog());
                CommunicationClient.Send(request);
            }

            _playerCoordinator.NextState();
        }

        public void HandleConnectionError(CommunicationException e)
        {
            CommunicationClient.HandleCommunicationError(e);

            if (e.Severity == CommunicationException.ErrorSeverity.Temporary)
                return;

            _playerCoordinator = new PlayerCoordinator(_gameName, _color, _role);
            new Thread(() => CommunicationClient.Connect(HandleConnectionError, HandleResponse)).Start();
            CommunicationClient.Send(_playerCoordinator.NextMove());           
        }
    }
}