using System;
using System.Collections.Generic;
using System.Text;
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
        private Field OccupiedField { get; set; }

        public Player()
        { }

        public Player(Board board)
        {
            Board = board;
        }
    }
}
