using System;
using System.Collections.Generic;
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

        private Guid PlayerGuid { get; set; }
        private PlayerBoard PlayerBoard { get; set; }
        private ILogger _logger;

        private IStrategy PlayerStrategy { get; set; }

        public IPlayerBoard Board => PlayerBoard;

        public IClient CommunicationClient;

        private int GameId { get; set; }

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
            PlayerStrategy = new PlayerStrategy(board, this, guid, GameId);
            PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);

            CommunicationClient = new AsynchronousClient(new PlayerConverter());
            CommunicationClient.SetupClient(HandleResponse);
            new Thread(() => CommunicationClient.StartClient()).Start();
        }

        public async Task InitializePlayer(int id, Guid guid, int gameId, TeamColor team, PlayerType role, PlayerBoard board,
            Location location)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(id);

            Id = id;
            Team = team;
            Role = role;
            PlayerGuid = guid;
            GameId = gameId;
            PlayerBoard = board;
            PlayerStrategy = new PlayerStrategy(board, this, guid, GameId);
            PlayerBoard.Players[id] = new PlayerInfo(id, team, role, location);

            await Task.Delay(10 * (id+1));
            CommunicationClient = new AsynchronousClient(new PlayerConverter());
            CommunicationClient.SetupClient(HandleResponse);
            new Thread(() => CommunicationClient.StartClient()).Start();
        }

        public IMessage GetNextRequestMessage()
        {
            return PlayerStrategy.NextMove();
        }

        private void HandleResponse(IMessage response)
        {
            //IMessage response;

            //while (!ResponsesQueue.TryDequeue(out response))
            //{
                //Task.Delay(10);
            //}
            //Log received response
            response.Process(this);
            //
            //change board state based on response 
            //  - update method in Response Message
            //based on board state change strategy state
            //  - implement strategy
            //  - hold current state
            //  - implement state changing action (stateless in next iteration) which return new message
            //
            //var message = new Move()
            //{
            //    PlayerId = player.Id,
            //    Direction = player.Team == TeamColor.Red ? Direction.Down : Direction.Up,
            //};

            //log current state

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