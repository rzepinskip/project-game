using System.Collections.Generic;
using System.Threading;
using Player.Strategy;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;

namespace Player
{
    public class Player : PlayerBase
    {
        public ObservableQueue<GameMessage> RequestsQueue { get; set; }
        public ObservableQueue<ResponseMessage> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private Board Board { get; set; }

        private List<PlayerBase> Players { get; set; }

        //public Location Location { get; set; }
        private PlayerStrategy PlayerStrategy { get; set; }

        public void InitializePlayer(int id, CommonResources.TeamColour team, PlayerType type, Board board,
            Location location)
        {
            Id = id;
            Team = team;
            Type = type;
            Board = board;
            //Location = location;
            PlayerStrategy = new PlayerStrategy(board, Team, Id);
            Board.Players.Add(id, new PlayerInfo(team, PlayerType.Leader, location));
        }

        public GameMessage GetNextRequestMessage()
        {
            var currentLocation = Board.Players[Id].Location;
            return PlayerStrategy.NextMove(currentLocation);
        }

        public void UpdateBoard(ResponseMessage responseMessage)
        {
            responseMessage.Update(Board);
        }


        public void HandleResponse(ResponseMessage response)
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
            //    Direction = player.Team == CommonResources.TeamColour.Red ? CommonResources.MoveType.Down : CommonResources.MoveType.Up,
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