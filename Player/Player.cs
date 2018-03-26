using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.BoardObjects;
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
        public ObservableConcurrentQueue<Request> RequestsQueue { get; set; }
        public ObservableConcurrentQueue<Response> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private PlayerBoard PlayerBoard { get; set; }
        private ILogger _logger;

        private IStrategy PlayerStrategy { get; set; }

        public IPlayerBoard Board => PlayerBoard;
 

        public void InitializePlayer(int id, string guid, TeamColor team, PlayerType role, PlayerBoard board,
            Location location)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(id);

            Id = id;
            Team = team;
            Role = role;
            PlayerGuid = guid;
            PlayerBoard = board;
            PlayerStrategy = new PlayerStrategy(board, Team, Id, guid);
            PlayerBoard.Players.Add(id, new PlayerInfo(id, team, role, location));
        }

        public IRequest GetNextRequestMessage()
        {
            var currentLocation = PlayerBoard.Players[Id].Location;
            return PlayerStrategy.NextMove(currentLocation);
        }

        private void HandleResponse()
        {
            Response response;

            while (!ResponsesQueue.TryDequeue(out response))
            {
                Task.Delay(10);
            }
            //Log received response
            response.Update(PlayerBoard);
            _logger.Info("RESPONSE: " + response.ToLog());
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
                _logger.Info("REQUEST: " + request.ToLog());
                RequestsQueue.Enqueue(request);
            }
            catch(StrategyException s)
            {
                //log exception
                _logger.Error("Thrown Exception", s);
                throw;
            }
        }

        public void StartListeningToResponses()
        {
            ResponsesQueue.ItemEnqueued += (sender, args) => { Task.Run(() => HandleResponse()); };
        }
    }
}