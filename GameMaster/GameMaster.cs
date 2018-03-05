using System;
using System.Collections.Generic;
using System.Text;
using Shared.Board;

namespace GameMaster
{
    class GameMaster
    {
        Dictionary<string, Pawn> Players { get; }
        Board Board { get; set; }
    }
}
