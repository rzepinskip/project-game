using System.Collections.Generic;
using System.Threading.Tasks;
using Common;
using Common.BoardObjects;
using Messaging.Requests;
using Messaging.Responses;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase
    {
        public ObservableConcurrentQueue<Request> RequestsQueue { get; set; }
        public ObservableConcurrentQueue<Response> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private PlayerBoard PlayerBoard { get; set; }

        private List<PlayerBase> Players { get; set; }

        //public Location Location { get; set; }
        private IStrategy PlayerStrategy { get; set; }

        public void InitializePlayer(int id, TeamColor team, PlayerType type, PlayerBoard board,
            Location location)
        {
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

            if (!ResponsesQueue.TryDequeue(out response)) throw new ConcurrencyException();

            response.Update(PlayerBoard);
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
            RequestsQueue.Enqueue(GetNextRequestMessage());
        }

        public void StartListeningToResponses()
        {
            ResponsesQueue.FirstItemEnqueued += (sender, args) => { Task.Run(() => HandleResponse()); };
        }
    }
}