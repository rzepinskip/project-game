using System.Collections.Generic;
using System.Threading;
using Common;
using Common.BoardObjects;
using Messaging;
using Messaging.Requests;
using Messaging.Responses;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase
    {
        public ObservableQueue<Request> RequestsQueue { get; set; }
        public ObservableQueue<Response> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private PlayerBoard PlayerBoard { get; set; }

        private List<PlayerBase> Players { get; set; }

        //public Location Location { get; set; }
        private PlayerStrategy PlayerStrategy { get; set; }

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

        public void UpdateBoard(Response responseMessage)
        {
            responseMessage.Update(PlayerBoard);
        }


        public void HandleResponse(Response response)
        {
            UpdateBoard(response);
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
            ResponsesQueue.CollectionChanged += (sender, args) =>
            {
                new Thread(() => HandleResponse(ResponsesQueue.Dequeue())).Start();
            };
        }
    }
}