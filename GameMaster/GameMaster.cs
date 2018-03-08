using System;
using System.Collections.Generic;
using System.Text;
using Shared.BoardObjects;

namespace GameMaster
{
    class GameMaster
    {
        Dictionary<string, Location> Players { get; }
        Board Board { get; set; }
    }
}
