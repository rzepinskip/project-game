using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Common;
using Common.BoardObjects;
using Common.Interfaces;
using Communication;
using Communication.Client;
using NLog;
using Player.Logging;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase, IPlayer
    {
        private bool _hasGameEnded;
        private ILogger _logger;
        private PlayerCoordinator _playerCoordinator;

        public Player(IMessageDeserializer messageDeserializer)
        {
            CommunicationClient = new AsynchronousClient(new TcpSocketConnector(messageDeserializer, HandleResponse));
        }

        public int GameId { get; private set; }
        public PlayerBoard PlayerBoard { get; private set; }
        public Guid PlayerGuid { get; private set; }

        public IClient CommunicationClient { get; }
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
            Location location)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(id);

            Id = id;
            Team = team;
            Role = role;
            PlayerGuid = guid;
            GameId = 0;
            PlayerBoard = board;
            PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);

            _playerCoordinator = new PlayerCoordinator("", team, role);

            new Thread(() => CommunicationClient.Connect()).Start();
        }

        public void InitializePlayer(string gameName, TeamColor color, PlayerType role)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(0);

            _playerCoordinator = new PlayerCoordinator(gameName, color, role);

            new Thread(() => CommunicationClient.Connect()).Start();
        }

        public IMessage GetNextRequestMessage()
        {
            return _playerCoordinator.NextMove();
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
                _logger.Info(request);
                CommunicationClient.Send(request);
            }

            _playerCoordinator.NextState();
        }
    }
}