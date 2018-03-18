using System.Collections.Generic;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;
using Player.Strategy;

namespace Player
{
    public class Player : PlayerBase
    {
        public Queue<GameMessage> RequestsQueue { get; set; }
        public Queue<ResponseMessage> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private Board Board { get; set; }
        private List<PlayerBase> Players { get; set; }
        //public Location Location { get; set; }
        private PlayerStrategy PlayerStrategy { get; set; }

        public void InitializePlayer(int id, CommonResources.TeamColour team, PlayerType type, Board board, Location location)
        {
            Id = id;
            Team = team;
            Type = type;
            Board = board;
            //Location = location;
            this.PlayerStrategy = new PlayerStrategy(board, Team, Id);
            this.Board.Players.Add(id, new PlayerInfo(team, PlayerType.Leader, location));
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

    }
}
