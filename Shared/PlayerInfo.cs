using Shared.BoardObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Shared
{
    public class PlayerInfo
    {
        public CommonResources.TeamColour Team { get; set; }
        public Location Location { get; set; }
        public Piece Piece { get; set; }

    }
}
