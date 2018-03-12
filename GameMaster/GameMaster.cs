using System;
using System.Collections.Generic;
using System.Text;
using Shared.Board;

namespace GameMaster
{
    class GameMaster
    {
        Dictionary<string, Location> Players { get; }
        Dictionary<string, int> PlayerGuidToId { get; }
        Dictionary<string, int> PlayerGuidToPieceId { get;  }
        Board Board { get; set; }
    }
}
