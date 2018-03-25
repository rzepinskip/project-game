using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;
using NLog;
using Player.Logging;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase
    {
        public ObservableConcurrentQueue<Request> RequestsQueue { get; set; }
        public ObservableConcurrentQueue<Response> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private PlayerBoard PlayerBoard { get; set; }
        private ILogger _logger;

        private List<PlayerBase> Players { get; set; }

        //public Location Location { get; set; }
        private IStrategy PlayerStrategy { get; set; }

        public void InitializePlayer(int id, TeamColor team, PlayerType type, PlayerBoard board,
            Location location)
        {
            var factory = new LoggerFactory();
            _logger = factory.GetPlayerLogger(id);

            Id = id;
            Team = team;
            Type = type;
            PlayerBoard = board;
            //Location = location;
            PlayerStrategy = new PlayerStrategy(board, Team, Id);
            PlayerBoard.Players.Add(id, new PlayerInfo(team, PlayerType.Leader, location));
        }

        public Request GetNextRequestMessage()
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