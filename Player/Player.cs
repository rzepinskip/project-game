using System.Collections.Generic;
using Shared;
using Shared.BoardObjects;
using Shared.GameMessages;
using Shared.ResponseMessages;

namespace Player
{
    public class Player : PlayerBase
    {
        public Queue<GameMessage> RequestsQueue { get; set; }
        public Queue<ResponseMessage> ResponsesQueue { get; set; }

        private string PlayerGuid { get; set; }
        private Board Board { get; set; }
        private List<PlayerBase> Players { get; set; }
        public Location Location { get; set; }

        public void InitializePlayer(int id, CommonResources.TeamColour team, PlayerType type, Board board, Location location)
        {
            Id = id;
            Team = team;
            Type = type;
            Board = board;
            Location = location;
        }
    }
}
