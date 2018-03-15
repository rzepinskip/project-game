using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;

namespace Shared.ResponseMessages
{
    public class TestPieceResponse : ResponseMessage
    {
        public Piece Piece { get; set; }
        public override void Update(Board board)
        {
            var playerInfo = board.Players[this.PlayerId];
            playerInfo.Piece = Piece;
        }
    }
}
