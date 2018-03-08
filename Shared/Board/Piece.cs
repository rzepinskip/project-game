using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Board
{
    class Piece 
    {
        public int Id { get; set; }
        public CommonResources.PieceType Type { get; set; }
        public DateTime TimeStamp { get; set; }
        public int? PlayerId { get; set; }

    }
}
