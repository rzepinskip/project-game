using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.BoardObjects;
using Common.Communication;
using Common.Interfaces;
using Messaging.Requests;
using Messaging.Responses;
using NLog;
using Player.Logging;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase, IPlayer
    {
        public ObservableConcurrentQueue<IRequest> RequestsQueue { get; set; }
        public ObservableConcurrentQueue<IMessage> ResponsesQueue { get; set; }

        public Guid PlayerGuid { get; set; }
        public PlayerBoard PlayerBoard { get; set; }
        private ILogger _logger;
        private PlayerCoordinator PlayerCoordinator { get; set; }
        public IPlayerBoard Board => PlayerBoard;

        private bool _hasGameEnded;
        public void UpdateGameState(IEnumerable<GameInfo> gameInfo)
        {
            PlayerCoordinator.UpdateGameStateInfo(gameInfo);
        }

        public void ChangePlayerCoordinatorState()
        {
            PlayerCoordinator.NextState();
        }

        public void UpdateJoiningInfo(bool info)
        {
            PlayerCoordinator.UpdateJoinInfo(info);
        }

        public void NotifyAboutGameEnd()
        {
            _hasGameEnded = true;
            PlayerCoordinator.NotifyAboutGameEnd();
        }

        public void UpdatePlayer(int playerId, Guid playerGuid, PlayerBase playerBase, int gameId)
        {
            Id = playerId;
            PlayerGuid = playerGuid;
            Team = playerBase.Team;
            Role = playerBase.Role;
            GameId = gameId;
        }

        public void UpdatePlayerGame(Location playerLocation, BoardInfo board)
        {
            PlayerBoard = new PlayerBoard(board.Width, board.TasksHeight, board.GoalsHeight);
            PlayerBoard.Players[Id] = new PlayerInfo(Id, Team, Role, playerLocation);
            Debug.WriteLine("Player is updating game");
            PlayerCoordinator.CreatePlayerStrategyFactory(new PlayerStrategyFactory(this));
        }

        public IClient CommunicationClient;

        public int GameId { get; set; }

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

            PlayerCoordinator = new PlayerCoordinator("game", team);

            CommunicationClient = new AsynchronousClient(new PlayerConverter());
            CommunicationClient.SetupClient(HandleResponse);
            new Thread(() => CommunicationClient.StartClient()).Start();
        }

        public void InitializePlayer(TeamColor color)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(0);

            //Id = id;
            //Team = team;
            //Role = role;
            //PlayerGuid = guid;
            //GameId = gameId;
            //PlayerBoard = board;
            //PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);

            PlayerCoordinator = new PlayerCoordinator("game", color);

            //await Task.Delay(10 * (id+1));
            CommunicationClient = new AsynchronousClient(new PlayerConverter());
            CommunicationClient.SetupClient(HandleResponse);
            new Thread(() => CommunicationClient.StartClient()).Start();
        }

        public IMessage GetNextRequestMessage()
        {
            return PlayerCoordinator.NextMove();
        }

        private void HandleResponse(IMessage response)
        {
            if (_hasGameEnded)
            {
                _hasGameEnded = true;
                return;
            }

            if (!response.Process(this))
                return;

           

            try
            {
                var request = GetNextRequestMessage();
                //RequestsQueue.Enqueue(request);
                _logger.Info(request);
                CommunicationClient.Send(request);
            }
            catch (StrategyException s)
            {
                //log exception
                throw;
            }
        }

        //public void StartListeningToResponses()
        //{
        //    ResponsesQueue.ItemEnqueued += (sender, args) => { Task.Run(() => HandleResponse()); };
        //}
    }
}